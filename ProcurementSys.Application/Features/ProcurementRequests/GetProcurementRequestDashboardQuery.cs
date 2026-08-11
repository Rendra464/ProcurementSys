using MediatR;
using ProcurementSys.Application.Features.ProcurementRequests.Dtos;
using ProcurementSys.Application.Features.ProcurementRequests.Services;

namespace ProcurementSys.Application.Features.ProcurementRequests;

// type = "Listing" | "NonListing" | null (gabungan, sesuai requirement dashboard FIT jadi satu dgn filter)
public record GetProcurementDashboardQuery(string? Type) : IRequest<List<ProcurementDashboardItemDto>>;

public class GetProcurementDashboardQueryHandler
    : IRequestHandler<GetProcurementDashboardQuery, List<ProcurementDashboardItemDto>>
{
    private readonly IProcurementRequestService _service;
    public GetProcurementDashboardQueryHandler(IProcurementRequestService service) => _service = service;

    public Task<List<ProcurementDashboardItemDto>> Handle(GetProcurementDashboardQuery request, CancellationToken ct)
        => _service.GetDashboardAsync(request.Type, ct);
}
