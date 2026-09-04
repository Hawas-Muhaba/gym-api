using MediatR;

namespace GymSystem.Application.Features.Appointments.DeleteAppointment;

public record DeleteAppointmentCommand(int AppointmentId) : IRequest;