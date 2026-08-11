using MediatR;

namespace ProcurementSys.Application.Features.ProcurementRequests.VendorAccess;

// Endpoint anonymous (tanpa JWT) yang diakses vendor dari link email, validasi Token ke VendorApprovalTokens.
public record SubmitVendorApprovalCommand(string Token, bool Approved, string? Notes) : IRequest<bool>;

// TODO: implement handler - validasi token belum expired & belum used, lalu update Assignment terkait
