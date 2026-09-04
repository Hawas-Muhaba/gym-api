using GymSystem.Application.Features.Clients.CreateClient;
using GymSystem.Application.Features.Clients.UpdateClient;
using GymSystem.Application.Features.Clients.GetClientById;
using GymSystem.Application.Features.Clients.GetClients;    
using GymSystem.Application.Features.Clients.DeleteClient;
using GymSystem.Application.Features.Clients.GetMyClientProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GymSystem.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize] // all endpoints in this controller require authentication
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ClientsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Staff,Manager")] // only staff and managers can create clients
    public async Task<IActionResult> Create(CreateClientCommand command)
     => Ok(await _mediator.Send(command));
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClientBody body)
    {
        await _mediator.Send(new UpdateClientCommand(id, body.FullName, body.Phone, body.Email, body.Notes));
        return NoContent();
    }
    [HttpGet]
    [Authorize(Roles = "Staff,Manager")] // clients shouldn't browse the full client list
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
        => Ok(await _mediator.Send(new GetClientsQuery(pageNumber, pageSize, search)));
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetClientByIdQuery(id)));

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
        => Ok(await _mediator.Send(new GetMyClientProfileQuery()));
    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteClientCommand(id));
        return NoContent();
    }
}

public record UpdateClientBody(string FullName, string Phone, string Email, string? Notes);