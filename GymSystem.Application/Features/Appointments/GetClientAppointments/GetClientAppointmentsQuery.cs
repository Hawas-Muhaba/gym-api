using MediatR;

namespace GymSystem.Application.Features.Appointments.GetClientAppointments;

// Notice: no Validator needed here — reads rarely need business validation,
// just a valid ClientId, which the route itself enforces.
public record GetClientAppointmentsQuery(int ClientId) : IRequest<List<AppointmentDto>>;