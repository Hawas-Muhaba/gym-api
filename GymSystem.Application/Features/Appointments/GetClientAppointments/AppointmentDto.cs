namespace GymSystem.Application.Features.Appointments.GetClientAppointments;

public record AppointmentDto(
    int Id,
    string StaffName,
    string ServiceName,
    DateTime StartTime,
    string Status
);