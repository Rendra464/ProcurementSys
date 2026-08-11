using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProcurementSys.Application.Features.ProcurementRequests;
using ProcurementSys.Application.Features.Workflow;

namespace ProcurementSys.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcurementRequestsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProcurementRequestsController(IMediator mediator) => _mediator = mediator;

    // ?type=Listing | NonListing | (kosong = gabungan keduanya)
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard([FromQuery] string? type)
        => Ok(await _mediator.Send(new GetProcurementDashboardQuery(type)));

    [HttpPost("assignments/{assignmentId}/approve")]
    public async Task<IActionResult> Approve(int assignmentId, [FromBody] string? notes)
    {
        // TODO: actionByUserId dari ICurrentUserService, bukan hardcode
        var success = await _mediator.Send(new ApproveAssignmentCommand(assignmentId, 0, notes));
        return success ? NoContent() : Conflict("Assignment sudah diproses atau bukan giliran kamu.");
    }
}
