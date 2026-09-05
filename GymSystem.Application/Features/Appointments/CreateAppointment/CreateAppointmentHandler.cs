using GymSystem.Application.Common.Interfaces;
using GymSystem.Domain.Entities;
using GymSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Appointments.CreateAppointment;
public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IBookingNotifier _notifier;
    private readonly ICurrentUserService _currentUserService;

    public CreateAppointmentHandler(
        IApplicationDbContext context,
        IBookingNotifier notifier,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _notifier = notifier;
        _currentUserService = currentUserService;
    }

public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
{
    var clientName = await _context.Clients
        .Where(client => client.Id == request.ClientId)
        .Select(client => client.FullName)
        .SingleOrDefaultAsync(cancellationToken) ?? "A client";
    var serviceName = await _context.Services
        .Where(service => service.Id == request.ServiceId)
        .Select(service => service.Name)
        .SingleOrDefaultAsync(cancellationToken) ?? "an appointment";

    var startTimeUtc = request.StartTime.Kind == DateTimeKind.Utc
        ? request.StartTime
        : DateTime.SpecifyKind(request.StartTime, DateTimeKind.Utc);
    var endTimeUtc = startTimeUtc.AddMinutes(60);

    var conflict = await _context.Appointments
        .AnyAsync(a => a.StaffId == request.StaffId
                    && a.Status != BookingStatus.Cancelled
                    && a.StartTime < endTimeUtc
                    && startTimeUtc < a.EndTime, cancellationToken);

    if (conflict)
        throw new InvalidOperationException("This staff member already has an appointment at that time.");

    var isStaffOrManager = _currentUserService.IsInRole("Staff") || _currentUserService.IsInRole("Manager");
    var initialStatus = isStaffOrManager ? BookingStatus.Confirmed : BookingStatus.Pending;

    var appointment = new Appointment
    {
        ClientId = request.ClientId,
        StaffId = request.StaffId,
        ServiceId = request.ServiceId,
        StartTime = startTimeUtc,
        EndTime = endTimeUtc,
        Status = initialStatus
    };

    _context.Appointments.Add(appointment);

    try
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
    {
        // The application-level check above passed, but the database caught a true race condition —
        // this is the "fire-code sensor" catching what the "door checker" missed.
        throw new InvalidOperationException("This slot was just booked by someone else. Please choose another time.");
    }

    await _notifier.NotifyNewBooking(request.StaffId, new
    {
        AppointmentId = appointment.Id,
        ClientName = clientName,
        ServiceName = serviceName,
        appointment.StartTime,
        appointment.Status
    }, cancellationToken);
    return appointment.Id;
}

private static bool IsUniqueConstraintViolation(DbUpdateException ex)
{
    // SQL Server error 2601/2627 = unique constraint/index violation — a specific, checkable signal
    return ex.InnerException?.Message.Contains("duplicate key") == true
        || ex.InnerException?.Message.Contains("unique index") == true;
}
}