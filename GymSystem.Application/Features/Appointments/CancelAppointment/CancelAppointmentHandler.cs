using GymSystem.Application.Common.Interfaces;
using GymSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Appointments.CancelAppointment;

public class CancelAppointmentHandler : IRequestHandler<CancelAppointmentCommand>
{
    private readonly IApplicationDbContext _context;
    public CancelAppointmentHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken)
            ?? throw new KeyNotFoundException("Appointment not found.");

        appointment.Status = BookingStatus.Cancelled;
        await _context.SaveChangesAsync(cancellationToken);
    }
}