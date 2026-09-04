using GymSystem.Application.Features.Services.CreateService;
using GymSystem.Application.Features.Services.GetServices;
using GymSystem.Application.Features.Services.DeleteService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ServicesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Create(CreateServiceCommand command)
        => Ok(await _mediator.Send(command));

    [HttpGet]
    [AllowAnonymous] // clients browsing services before logging in — a reasonable public endpoint
    public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetServicesQuery()));

    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteServiceCommand(id));
        return NoContent();
    }
}