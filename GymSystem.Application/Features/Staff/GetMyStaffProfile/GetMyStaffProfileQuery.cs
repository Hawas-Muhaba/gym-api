using MediatR;

namespace GymSystem.Application.Features.Staff.GetMyStaffProfile;

public record GetMyStaffProfileQuery : IRequest<MyStaffProfileDto>;

public record MyStaffProfileDto(
    int Id,
    string FullName,
    string Phone,
    decimal CommissionRate
);
