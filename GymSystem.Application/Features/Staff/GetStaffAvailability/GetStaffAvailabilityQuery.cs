using MediatR;

namespace GymSystem.Application.Features.Staff.GetStaffAvailability;

public record GetStaffAvailabilityQuery(int StaffId, DateTime Date) : IRequest<List<TimeSlotDto>>;

public record TimeSlotDto(DateTime StartTime, bool IsAvailable);