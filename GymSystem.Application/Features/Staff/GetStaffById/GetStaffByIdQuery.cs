using MediatR;

namespace GymSystem.Application.Features.Staff.GetStaffById;

public record GetStaffByIdQuery(int StaffId) : IRequest<StaffDetailDto>;

public record StaffDetailDto(int Id, string FullName, string Phone, decimal CommissionRate, int TotalAppointments);