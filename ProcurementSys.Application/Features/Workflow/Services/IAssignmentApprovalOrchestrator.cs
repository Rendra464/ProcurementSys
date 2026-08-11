using ProcurementSys.Domain.Entities;
using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Application.Features.Workflow.Services;

public interface IAssignmentApprovalOrchestrator
{
    Task<Assignment?> StartApprovalAsync(
        WorkflowProcessType entityType,
        int entityId,
        decimal entityValue,
        CancellationToken cancellationToken = default);

    Task<Assignment?> AdvanceApprovalAsync(
        Assignment completedAssignment,
        decimal entityValue,
        CancellationToken cancellationToken = default);
}