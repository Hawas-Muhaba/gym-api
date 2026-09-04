using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Staff.GetStaffById;

public class GetStaffByIdHandler : IRequestHandler<GetStaffByIdQuery, StaffDetailDto>
{
    private readonly IApplicationDbContext _context;
    public GetStaffByIdHandler(IApplicationDbContext context) => _context = context;

    public async Task<StaffDetailDto> Handle(GetStaffByIdQuery request, CancellationToken cancellationToken)
    {
        var staff = await _context.Staffs
            .Include(s => s.Appointments)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.StaffId, cancellationToken)
            ?? throw new KeyNotFoundException("Staff member not found.");

        return new StaffDetailDto(staff.Id, staff.FullName, staff.Phone, staff.CommissionRate, staff.Appointments.Count);
    }
}