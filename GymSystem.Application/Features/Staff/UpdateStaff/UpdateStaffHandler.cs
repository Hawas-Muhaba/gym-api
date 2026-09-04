using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Staff.UpdateStaff;

public class UpdateStaffHandler : IRequestHandler<UpdateStaffCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateStaffHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.Id == request.StaffId, cancellationToken)
            ?? throw new KeyNotFoundException("Staff member not found.");

        staff.FullName = request.FullName;
        staff.Phone = request.Phone;
        staff.CommissionRate = request.CommissionRate;

        await _context.SaveChangesAsync(cancellationToken);
    }
}