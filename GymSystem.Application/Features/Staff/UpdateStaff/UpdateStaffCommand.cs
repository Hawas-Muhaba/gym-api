using MediatR;

namespace GymSystem.Application.Features.Staff.UpdateStaff;

public record UpdateStaffCommand(int StaffId, string FullName, string Phone, decimal CommissionRate) : IRequest;