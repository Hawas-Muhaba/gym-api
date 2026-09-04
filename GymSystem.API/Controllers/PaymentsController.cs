using GymSystem.Application.Features.Payments.RecordPayment;
using GymSystem.Application.Features.Payments.GetStaffCommissionReport;
using GymSystem.Application.Features.Payments.DeletePayment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Staff,Manager")] // clients never touch payment endpoints directly
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public PaymentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Record(RecordPaymentCommand command)
        => Ok(await _mediator.Send(command));

    [HttpGet("staff/{staffId}/commission-report")]
    [Authorize(Roles = "Manager")] // staff shouldn't see each other's commission — Manager only
    public async Task<IActionResult> GetCommissionReport(int staffId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        => Ok(await _mediator.Send(new GetStaffCommissionReportQuery(staffId, fromDate, toDate)));

    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeletePaymentCommand(id));
        return NoContent();
    }
}