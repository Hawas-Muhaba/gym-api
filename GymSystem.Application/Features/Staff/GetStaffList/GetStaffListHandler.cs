using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Staff.GetStaffList;

public class GetStaffListHandler : IRequestHandler<GetStaffListQuery, List<StaffListDto>>
{
    private readonly IApplicationDbContext _context;
    public GetStaffListHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<StaffListDto>> Handle(GetStaffListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Staffs
            .AsNoTracking()
            .OrderBy(s => s.FullName)
            .Select(s => new StaffListDto(s.Id, s.FullName, s.Phone, s.CommissionRate))
            .ToListAsync(cancellationToken);
    }
}