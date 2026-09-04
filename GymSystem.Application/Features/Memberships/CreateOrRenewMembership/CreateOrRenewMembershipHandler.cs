using GymSystem.Application.Common.Interfaces;
using GymSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Memberships.CreateOrRenewMembership;

public class CreateOrRenewMembershipHandler : IRequestHandler<CreateOrRenewMembershipCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateOrRenewMembershipHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(CreateOrRenewMembershipCommand request, CancellationToken cancellationToken)
    {
        var membership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.ClientId == request.ClientId, cancellationToken);

        // Renewal rule: if still active, extend FROM the current expiry (don't waste remaining days).
        // If expired or brand new, start fresh from today.
        var baseDate = membership is { IsActive: true } ? membership.ExpiryDate : DateTime.UtcNow;
        var newExpiry = baseDate.AddMonths(request.DurationMonths);

        if (membership is null)
        {
            membership = new Membership
            {
                ClientId = request.ClientId,
                StartDate = DateTime.UtcNow,
                ExpiryDate = newExpiry
            };
            _context.Memberships.Add(membership);
        }
        else
        {
            membership.ExpiryDate = newExpiry;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return membership.Id;
    }
}