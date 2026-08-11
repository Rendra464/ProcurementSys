namespace ProcurementSys.Application.Features.ProcurementRequests.Dtos;

public record ProcurementDashboardItemDto(
    int Id, string ProcurementNumber, string Type, string Status,
    string MaterialRequestNumber, string VendorName, int ApprovedSteps, int TotalSteps);
