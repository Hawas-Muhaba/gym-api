using GymSystem.Application.Features.Appointments.CreateAppointment;
using GymSystem.Application.Features.Appointments.CancelAppointment;
using GymSystem.Application.Features.Appointments.UpdateAppointmentStatus;
using GymSystem.Application.Features.Appointments.GetClientAppointments;
using GymSystem.Application.Features.Appointments.GetStaffSchedule;
using GymSystem.Application.Features.Appointments.GetAppointmentById;
using GymSystem.Application.Features.Appointments.DeleteAppointment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using GymSystem.Domain.Enums;

namespace GymSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // must be logged in for anything in this controller
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AppointmentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [EnableRateLimiting("booking")]
    public async Task<IActionResult> Create(CreateAppointmentCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetAppointmentByIdQuery(id)));

    [HttpGet("client/{clientId}")]
    public async Task<IActionResult> GetByClient(int clientId)
        => Ok(await _mediator.Send(new GetClientAppointmentsQuery(clientId)));

    [HttpGet("staff/{staffId}/schedule")]
    public async Task<IActionResult> GetStaffSchedule(int staffId, [FromQuery] DateTime date)
        => Ok(await _mediator.Send(new GetStaffScheduleQuery(staffId, date)));

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        await _mediator.Send(new CancelAppointmentCommand(id));
        return NoContent(); // 204 — standard REST response for "done, nothing to return"
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Staff,Manager")] // clients shouldn't mark their own appointment "Completed"
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] BookingStatus newStatus)
    {
        await _mediator.Send(new UpdateAppointmentStatusCommand(id, newStatus));
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteAppointmentCommand(id));
        return NoContent();
    }
}