using MediatR;

namespace GymSystem.Application.Features.Appointments.GetAppointmentById;

public record GetAppointmentByIdQuery(int AppointmentId) : IRequest<AppointmentDetailDto>;

public record AppointmentDetailDto(
    int Id,
    string ClientName,
    string StaffName,
    string ServiceName,
    DateTime StartTime,
    DateTime EndTime,
    string Status
);