using GymSystem.Application.Features.Auth.Register;
using GymSystem.Application.Features.Auth.Login;
using GymSystem.Application.Features.Auth.Refresh;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous] // obviously — you can't log in if you must already be logged in
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
        => Ok(new { userId = await _mediator.Send(command) });

    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login(LoginCommand command)
        => Ok(new { token = await _mediator.Send(command) });
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshCommand command)
        => Ok(await _mediator.Send(command));
}