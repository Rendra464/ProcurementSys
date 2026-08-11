using MediatR;
using ProcurementSys.Application.Features.Workflow.Services;

namespace ProcurementSys.Application.Features.Workflow;

public record ApproveAssignmentCommand(int AssignmentId, int ActionByUserId, string? Notes) : IRequest<bool>;

public class ApproveAssignmentCommandHandler : IRequestHandler<ApproveAssignmentCommand, bool>
{
    private readonly IApprovalWorkflowService _workflowService;
    public ApproveAssignmentCommandHandler(IApprovalWorkflowService workflowService) => _workflowService = workflowService;

    public Task<bool> Handle(ApproveAssignmentCommand request, CancellationToken ct)
        => _workflowService.ApproveAsync(request.AssignmentId, request.ActionByUserId, request.Notes, ct);
}
