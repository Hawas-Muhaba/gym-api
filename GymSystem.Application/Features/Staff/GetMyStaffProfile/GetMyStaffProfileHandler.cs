using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Staff.GetMyStaffProfile;

public class GetMyStaffProfileHandler : IRequestHandler<GetMyStaffProfileQuery, MyStaffProfileDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyStaffProfileHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<MyStaffProfileDto> Handle(GetMyStaffProfileQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(currentUserId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var staff = await _context.Staffs
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == currentUserId, cancellationToken)
            ?? throw new KeyNotFoundException("Staff profile not found for the current user.");

        return new MyStaffProfileDto(
            staff.Id,
            staff.FullName,
            staff.Phone,
            staff.CommissionRate
        );
    }
}
