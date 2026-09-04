using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Staff.DeleteStaff;

public class DeleteStaffHandler : IRequestHandler<DeleteStaffCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStaffHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
    {
        var staff = await _context.Staffs
            .FirstOrDefaultAsync(s => s.Id == request.StaffId, cancellationToken)
            ?? throw new KeyNotFoundException("Staff member not found.");

        staff.IsDeleted = true;
        staff.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}