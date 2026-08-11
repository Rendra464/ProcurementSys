using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcurementSys.Application.Features.ProcurementRequests.VendorAccess;

namespace ProcurementSys.Api.Controllers;

// Endpoint khusus vendor NON-LISTING yang akses via magic-link email, TANPA login ke aplikasi.
[ApiController]
[Route("api/vendor-approval")]
[AllowAnonymous]
public class VendorApprovalController : ControllerBase
{
    private readonly IMediator _mediator;
    public VendorApprovalController(IMediator mediator) => _mediator = mediator;

    [HttpPost("{token}")]
    public async Task<IActionResult> Submit(string token, [FromBody] bool approved, [FromQuery] string? notes)
    {
        var success = await _mediator.Send(new SubmitVendorApprovalCommand(token, approved, notes));
        return success ? Ok() : BadRequest("Token tidak valid atau sudah kedaluwarsa.");
    }
}
