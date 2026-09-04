using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Clients.GetClients;

public class GetClientsHandler : IRequestHandler<GetClientsQuery, PagedResult<ClientListDto>>
{
    private readonly IApplicationDbContext _context;
    public GetClientsHandler(IApplicationDbContext context) => _context = context;

    public async Task<PagedResult<ClientListDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Clients.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(c => c.FullName.Contains(request.Search) || c.Phone.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);
        var now = DateTime.UtcNow;

        var items = await query
            .OrderBy(c => c.FullName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new ClientListDto(c.Id, c.FullName, c.Phone, c.Memberships.Any(m => m.ExpiryDate >= now)))
            .ToListAsync(cancellationToken);

        return new PagedResult<ClientListDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}