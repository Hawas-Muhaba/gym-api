using GymSystem.Application.Features.Memberships.CreateOrRenewMembership;
using GymSystem.Application.Features.Memberships.GetExpiringSoon;
using GymSystem.Application.Features.Memberships.GetMembershipByClientId;
using GymSystem.Application.Features.Memberships.DeleteMembership;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembershipsController : ControllerBase
{
    private readonly IMediator _mediator;
    public MembershipsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("renew")]
    [Authorize(Roles = "Staff,Manager")]
    public async Task<IActionResult> CreateOrRenew(CreateOrRenewMembershipCommand command)
        => Ok(await _mediator.Send(command));

    [HttpGet("client/{clientId}")]
    public async Task<IActionResult> GetByClient(int clientId)
        => Ok(await _mediator.Send(new GetMembershipByClientIdQuery(clientId)));

    [HttpGet("expiring-soon")]
    [Authorize(Roles = "Staff,Manager")]
    public async Task<IActionResult> GetExpiringSoon([FromQuery] int withinDays = 7)
        => Ok(await _mediator.Send(new GetExpiringSoonQuery(withinDays)));

    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteMembershipCommand(id));
        return NoContent();
    }
}