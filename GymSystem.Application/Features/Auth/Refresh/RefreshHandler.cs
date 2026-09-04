using GymSystem.Application.Common.Interfaces;
using MediatR;

namespace GymSystem.Application.Features.Auth.Refresh;

public class RefreshHandler : IRequestHandler<RefreshCommand, TokenResult>
{
    private readonly IIdentityService _identityService;
    public RefreshHandler(IIdentityService identityService) => _identityService = identityService;

    public async Task<TokenResult> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var (succeeded, tokens) = await _identityService.RefreshAsync(request.AccessToken, request.RefreshToken);
        if (!succeeded)
            throw new UnauthorizedAccessException("Invalid or expired refresh token. Please log in again.");

        return tokens!;
    }
}