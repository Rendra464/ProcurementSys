using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Domain.Entities;

public class ApprovalHistory
{
    public int Id { get; set; }
    public int AssignmentId { get; set; }
    public ApprovalAction Action { get; set; }
    public int ActionByUserId { get; set; }
    public DateTime ActionAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}
