using MediatR;

namespace GymSystem.Application.Features.Appointments.GetStaffSchedule;

public record GetStaffScheduleQuery(int StaffId, DateTime Date) : IRequest<List<ScheduleItemDto>>;

public record ScheduleItemDto(int AppointmentId, string ClientName, string ServiceName, DateTime StartTime, string Status, decimal ServicePrice, decimal CommissionRate);