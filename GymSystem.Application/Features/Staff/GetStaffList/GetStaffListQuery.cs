using MediatR;

namespace GymSystem.Application.Features.Staff.GetStaffList;

public record GetStaffListQuery : IRequest<List<StaffListDto>>;

public record StaffListDto(int Id, string FullName, string Phone, decimal CommissionRate);