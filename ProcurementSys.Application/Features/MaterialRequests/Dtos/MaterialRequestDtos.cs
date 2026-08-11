namespace ProcurementSys.Application.Features.MaterialRequests.Dtos;

public record CreateMaterialRequestItemDto(
    string ItemName,
    decimal Quantity,
    string Unit,
    decimal EstimatedUnitPrice
);