using GymSystem.Domain.Enums;
using MediatR;

namespace GymSystem.Application.Features.Appointments.UpdateAppointmentStatus;

public record UpdateAppointmentStatusCommand(int AppointmentId, BookingStatus NewStatus) : IRequest;