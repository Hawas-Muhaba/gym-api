using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Appointments.UpdateAppointmentStatus;

public class UpdateAppointmentStatusHandler : IRequestHandler<UpdateAppointmentStatusCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateAppointmentStatusHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(UpdateAppointmentStatusCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken)
            ?? throw new KeyNotFoundException("Appointment not found.");

        appointment.Status = request.NewStatus; // used for marking Completed or NoShow after the visit
        await _context.SaveChangesAsync(cancellationToken);
    }
}