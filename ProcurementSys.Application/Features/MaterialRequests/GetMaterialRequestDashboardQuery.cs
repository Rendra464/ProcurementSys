using MediatR;
using ProcurementSys.Application.Features.MaterialRequests.Dtos;
using ProcurementSys.Application.Features.MaterialRequests.Services;

namespace ProcurementSys.Application.Features.MaterialRequests;

public record GetMaterialRequestDashboardQuery(string? Status) : IRequest<List<MaterialRequestDashboardItemDto>>;

public class GetMaterialRequestDashboardQueryHandler
    : IRequestHandler<GetMaterialRequestDashboardQuery, List<MaterialRequestDashboardItemDto>>
{
    private readonly IMaterialRequestService _service;
    public GetMaterialRequestDashboardQueryHandler(IMaterialRequestService service) => _service = service;

    public Task<List<MaterialRequestDashboardItemDto>> Handle(GetMaterialRequestDashboardQuery request, CancellationToken ct)
        => _service.GetDashboardAsync(request.Status, ct);
}
