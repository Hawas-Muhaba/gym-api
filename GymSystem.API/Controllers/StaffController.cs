using GymSystem.Application.Features.Staff.CreateStaff;
using GymSystem.Application.Features.Staff.UpdateStaff;
using GymSystem.Application.Features.Staff.GetStaffList;
using GymSystem.Application.Features.Staff.GetStaffById;
using GymSystem.Application.Features.Staff.GetStaffAvailability;
using GymSystem.Application.Features.Staff.GetMyStaffProfile;
using GymSystem.Application.Features.Staff.DeleteStaff;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StaffController : ControllerBase
{
    private readonly IMediator _mediator;
    public StaffController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Manager")] // only managers hire/add staff
    public async Task<IActionResult> Create(CreateStaffCommand command)
        => Ok(await _mediator.Send(command));
    [HttpPut("{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStaffBody body)
    {
        await _mediator.Send(new UpdateStaffCommand(id, body.FullName, body.Phone, body.CommissionRate));
        return NoContent();
    }
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetStaffListQuery()));
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _mediator.Send(new GetStaffByIdQuery(id)));
    [HttpGet("{id}/availability")]
    public async Task<IActionResult> GetAvailability(int id, [FromQuery] DateTime date)
        => Ok(await _mediator.Send(new GetStaffAvailabilityQuery(id, date)));

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
        => Ok(await _mediator.Send(new GetMyStaffProfileQuery()));

    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteStaffCommand(id));
        return NoContent();
    }
}

public record UpdateStaffBody(string FullName, string Phone, decimal CommissionRate);