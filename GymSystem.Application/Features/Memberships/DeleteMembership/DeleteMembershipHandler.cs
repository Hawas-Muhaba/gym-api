using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Memberships.DeleteMembership;

public class DeleteMembershipHandler : IRequestHandler<DeleteMembershipCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteMembershipHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteMembershipCommand request, CancellationToken cancellationToken)
    {
        var membership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.Id == request.MembershipId, cancellationToken)
            ?? throw new KeyNotFoundException("Membership not found.");

        membership.IsDeleted = true;
        membership.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}