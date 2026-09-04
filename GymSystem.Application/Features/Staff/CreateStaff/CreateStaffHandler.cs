using GymSystem.Application.Common.Interfaces;
using MediatR;
using StaffEntity = GymSystem.Domain.Entities.Staff;

namespace GymSystem.Application.Features.Staff.CreateStaff;

public class CreateStaffHandler : IRequestHandler<CreateStaffCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateStaffHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        var staff = new StaffEntity
        {
            FullName = request.FullName,
            Phone = request.Phone,
            CommissionRate = request.CommissionRate
        };

        _context.Staffs.Add(staff);
        await _context.SaveChangesAsync(cancellationToken);
        return staff.Id;
    }
}