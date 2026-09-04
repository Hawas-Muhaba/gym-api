using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Clients.GetMyClientProfile;

public class GetMyClientProfileHandler : IRequestHandler<GetMyClientProfileQuery, MyClientProfileDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyClientProfileHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<MyClientProfileDto> Handle(GetMyClientProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(userId)) throw new UnauthorizedAccessException();

        var client = await _context.Clients.AsNoTracking()
            .FirstOrDefaultAsync(item => item.UserId == userId, cancellationToken)
            ?? throw new KeyNotFoundException("Client profile not found for the current user.");

        return new MyClientProfileDto(client.Id, client.FullName, client.Phone, client.Email);
    }
}