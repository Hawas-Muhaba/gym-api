using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Memberships.GetExpiringSoon;

public class GetExpiringSoonHandler : IRequestHandler<GetExpiringSoonQuery, List<ExpiringMembershipDto>>
{
    private readonly IApplicationDbContext _context;
    public GetExpiringSoonHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<ExpiringMembershipDto>> Handle(GetExpiringSoonQuery request, CancellationToken cancellationToken)
    {
        var cutoff = DateTime.UtcNow.AddDays(request.WithinDays);

        return await _context.Memberships
            .Where(m => m.ExpiryDate <= cutoff && m.ExpiryDate >= DateTime.UtcNow)
            .OrderBy(m => m.ExpiryDate)
            .Select(m => new ExpiringMembershipDto(
                m.ClientId,
                m.Client.FullName,
                m.Client.Phone,
                m.ExpiryDate,
                m.Client.TelegramChatId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}