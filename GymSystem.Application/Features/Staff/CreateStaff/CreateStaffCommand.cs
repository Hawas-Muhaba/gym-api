using MediatR;

namespace GymSystem.Application.Features.Staff.CreateStaff;

public record CreateStaffCommand(string FullName, string Phone, decimal CommissionRate) : IRequest<int>;