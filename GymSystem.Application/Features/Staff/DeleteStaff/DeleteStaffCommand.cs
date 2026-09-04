using MediatR;

namespace GymSystem.Application.Features.Staff.DeleteStaff;

public record DeleteStaffCommand(int StaffId) : IRequest;