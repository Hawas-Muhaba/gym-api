using MediatR;
using GymSystem.Application.Features.Appointments.CreateAppointment;

public record CreateAppointmentCommand(
    int ClientId,
    int StaffId,
    int ServiceId,
    DateTime StartTime
) : IRequest<int>;
