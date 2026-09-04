using MediatR;

namespace GymSystem.Application.Features.Appointments.CancelAppointment;

public record CancelAppointmentCommand(int AppointmentId) : IRequest;
// IRequest (no <T>) means "this returns nothing, just does the action"