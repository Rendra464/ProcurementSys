using ProcurementSys.Domain.Enums;
using ProcurementSys.Domain.Exceptions;

namespace ProcurementSys.Domain.Entities;

public class Assignment
{
    public int Id { get; set; }
    public WorkflowProcessType EntityType { get; private set; }
    public int EntityId { get; private set; }
    public int WorkflowStepId { get; private set; }
    public int? AssigneeUserId { get; private set; }
    public AssignmentStatus Status { get; private set; } = AssignmentStatus.Pending;
    public DateTime? ActionAt { get; private set; }
    public string? Notes { get; private set; }
    public byte[]? RowVersion { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    private Assignment() { }

    public static Assignment Create(WorkflowProcessType entityType, int entityId, int workflowStepId, int? assigneeUserId)
    {
        return new Assignment
        {
            EntityType = entityType,
            EntityId = entityId,
            WorkflowStepId = workflowStepId,
            AssigneeUserId = assigneeUserId,
            Status = AssignmentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }
    public void Approve(int actionByUserId, string? notes = null)
    {
        EnsurePending();
        EnsureAuthorizedAssignee(actionByUserId);
        ApplyApproval(notes);
    }

    public void Reject(int actionByUserId, string? notes = null)
    {
        EnsurePending();
        EnsureAuthorizedAssignee(actionByUserId);
        ApplyRejection(notes);
    }
    public void ApproveByToken(string? notes = null)
    {
        EnsurePending();
        ApplyApproval(notes);
    }

    public void RejectByToken(string? notes = null)
    {
        EnsurePending();
        ApplyRejection(notes);
    }

    public void Skip(string? reason = null)
    {
        EnsurePending();
        Status = AssignmentStatus.Skipped;
        ActionAt = DateTime.UtcNow;
        Notes = reason ?? "Skipped by system workflow evaluation.";
    }
    private void ApplyApproval(string? notes)
    {
        Status = AssignmentStatus.Approved;
        ActionAt = DateTime.UtcNow;
        Notes = notes;
    }

    private void ApplyRejection(string? notes)
    {
        Status = AssignmentStatus.Rejected;
        ActionAt = DateTime.UtcNow;
        Notes = notes;
    }
    private void EnsurePending()
    {
        if (Status != AssignmentStatus.Pending)
        {
            throw new AssignmentNotPendingException(Id, Status.ToString());
        }
    }

    private void EnsureAuthorizedAssignee(int actionByUserId)
    {
        if (AssigneeUserId.HasValue && AssigneeUserId.Value != actionByUserId)
        {
            throw new AssignmentUnauthorizedException(actionByUserId, Id, AssigneeUserId);
        }
    }
}