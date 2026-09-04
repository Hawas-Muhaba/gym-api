using GymSystem.Application.Common.Interfaces;
using MediatR;

namespace GymSystem.Application.Features.Auth.Login;

public class LoginHandler : IRequestHandler<LoginCommand, TokenResult>
{
    private readonly IIdentityService _identityService;
    public LoginHandler(IIdentityService identityService) => _identityService = identityService;

    public async Task<TokenResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (succeeded, tokens) = await _identityService.LoginAsync(request.Email, request.Password);
        if (!succeeded)
            throw new UnauthorizedAccessException("Invalid email or password.");

        return tokens!;
    }
}