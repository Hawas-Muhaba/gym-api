using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Services.GetServices;

public class GetServicesHandler : IRequestHandler<GetServicesQuery, List<ServiceDto>>
{
    private readonly IApplicationDbContext _context;
    public GetServicesHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<ServiceDto>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Services
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .Select(s => new ServiceDto(s.Id, s.Name, s.Price, s.DurationMinutes))
            .ToListAsync(cancellationToken);
    }
}