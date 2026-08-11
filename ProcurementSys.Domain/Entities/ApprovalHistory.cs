using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Domain.Entities;

public class ApprovalHistory
{
    public int Id { get; private set; }
    public int AssignmentId { get; private set; }
    public ApprovalAction Action { get; private set; }
    public int ActionByUserId { get; private set; }
    public string? Notes { get; private set; }
    public DateTime ActionDate { get; private set; }
    public Assignment? Assignment { get; private set; }
    private ApprovalHistory() { }
    public static ApprovalHistory Create(
        int assignmentId,
        ApprovalAction action,
        int actionByUserId,
        string? notes = null)
    {
        return new ApprovalHistory
        {
            AssignmentId = assignmentId,
            Action = action,
            ActionByUserId = actionByUserId,
            Notes = notes,
            ActionDate = DateTime.UtcNow
        };
    }
}