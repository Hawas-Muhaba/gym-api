using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Memberships.GetMembershipByClientId;

public class GetMembershipByClientIdHandler : IRequestHandler<GetMembershipByClientIdQuery, MembershipDto?>
{
    private readonly IApplicationDbContext _context;
    public GetMembershipByClientIdHandler(IApplicationDbContext context) => _context = context;

    public async Task<MembershipDto?> Handle(GetMembershipByClientIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Memberships
            .Where(m => m.ClientId == request.ClientId)
            .Select(m => new MembershipDto(m.Id, m.StartDate, m.ExpiryDate, m.ExpiryDate >= DateTime.UtcNow))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        // returns null if no membership exists yet — handled gracefully, not an exception,
        // since "no membership yet" is a normal state, not an error
    }
}