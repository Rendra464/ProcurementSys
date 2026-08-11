using MediatR;
using ProcurementSys.Application.Features.Workflow.Services;

namespace ProcurementSys.Application.Features.Workflow;

public record RejectAssignmentCommand(int AssignmentId, int ActionByUserId, string? Notes) : IRequest<bool>;

public class RejectAssignmentCommandHandler : IRequestHandler<RejectAssignmentCommand, bool>
{
    private readonly IApprovalWorkflowService _workflowService;
    public RejectAssignmentCommandHandler(IApprovalWorkflowService workflowService) => _workflowService = workflowService;

    public Task<bool> Handle(RejectAssignmentCommand request, CancellationToken ct)
        => _workflowService.RejectAsync(request.AssignmentId, request.ActionByUserId, request.Notes, ct);
}
