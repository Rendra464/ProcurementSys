using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Application.Common.Exceptions;
using ProcurementSys.Domain.Entities;
using ProcurementSys.Domain.Enums;
using ProcurementSys.Domain.Services;

namespace ProcurementSys.Application.Features.Workflow.Services;

public class AssignmentApprovalOrchestrator : IAssignmentApprovalOrchestrator
{
    private readonly IWorkflowDefinitionRepository _workflowDefinitionRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProcurementRequestRepository _procurementRequestRepository;
    private readonly ApprovalWorkflowService _approvalWorkflowService;

    public AssignmentApprovalOrchestrator(
        IWorkflowDefinitionRepository workflowDefinitionRepository,
        IAssignmentRepository assignmentRepository,
        IUserRepository userRepository,
        IProcurementRequestRepository procurementRequestRepository,
        ApprovalWorkflowService approvalWorkflowService)
    {
        _workflowDefinitionRepository = workflowDefinitionRepository;
        _assignmentRepository = assignmentRepository;
        _userRepository = userRepository;
        _procurementRequestRepository = procurementRequestRepository;
        _approvalWorkflowService = approvalWorkflowService;
    }

    public async Task<Assignment?> StartApprovalAsync(
        WorkflowProcessType entityType,
        int entityId,
        decimal entityValue,
        CancellationToken cancellationToken = default)
    {
        var definition = await GetWorkflowDefinitionAsync(entityType, cancellationToken);

        return await ProcessNextStepAsync(
            definition,
            entityType,
            entityId,
            currentStepOrder: 0,
            entityValue,
            cancellationToken);
    }

    public async Task<Assignment?> AdvanceApprovalAsync(
        Assignment completedAssignment,
        decimal entityValue,
        CancellationToken cancellationToken = default)
    {
        var definition = await GetWorkflowDefinitionAsync(completedAssignment.EntityType, cancellationToken);

        var currentStep = definition.Steps.FirstOrDefault(s => s.Id == completedAssignment.WorkflowStepId);
        if (currentStep is null)
        {
            throw new InvalidOperationException($"WorkflowStep dengan ID '{completedAssignment.WorkflowStepId}' tidak ditemukan di definisi workflow.");
        }

        return await ProcessNextStepAsync(
            definition,
            completedAssignment.EntityType,
            completedAssignment.EntityId,
            currentStepOrder: currentStep.StepOrder,
            entityValue,
            cancellationToken);
    }

    private async Task<WorkflowDefinition> GetWorkflowDefinitionAsync(WorkflowProcessType entityType, CancellationToken cancellationToken)
    {
        var definition = await _workflowDefinitionRepository.GetActiveDefinitionAsync(entityType, cancellationToken);
        if (definition is null)
        {
            throw new WorkflowDefinitionNotFoundException(entityType);
        }
        return definition;
    }

    private async Task<Assignment?> ProcessNextStepAsync(
        WorkflowDefinition definition,
        WorkflowProcessType entityType,
        int entityId,
        int currentStepOrder,
        decimal entityValue,
        CancellationToken cancellationToken)
    {
        var nextStep = _approvalWorkflowService.GetNextStep(definition, currentStepOrder, entityValue);
        if (nextStep is null)
        {
            return null; 
        }

        int? assigneeUserId = await ResolveAssigneeAsync(entityType, entityId, nextStep, cancellationToken);

        var assignment = Assignment.Create(
            entityType,
            entityId,
            nextStep.Id,
            assigneeUserId);

        await _assignmentRepository.AddAsync(assignment, cancellationToken);

        return assignment;
    }

    private async Task<int?> ResolveAssigneeAsync(
        WorkflowProcessType entityType,
        int entityId,
        WorkflowStep step,
        CancellationToken cancellationToken)
    {
        if (entityType == WorkflowProcessType.ProcurementNonListing && step.RequiredRole == UserRole.ApproverVendor)
        {
            return null;
        }

        if (entityType == WorkflowProcessType.ProcurementListing && step.RequiredRole == UserRole.ApproverVendor)
        {
            var pr = await _procurementRequestRepository.GetByIdAsync(entityId, cancellationToken);
            if (pr is null)
            {
                throw new NotFoundException(nameof(ProcurementRequest), entityId);
            }

            if (pr.VendorId <= 0)
            {
                throw new InvalidOperationException($"ProcurementRequest dengan ID '{entityId}' tidak memiliki VendorId yang valid.");
            }

            var vendorUser = await _userRepository.GetByVendorIdAsync(pr.VendorId, cancellationToken);
            if (vendorUser is null)
            {
                throw new NotFoundException($"User Representative untuk Vendor ID '{pr.VendorId}'", pr.VendorId);
            }

            return vendorUser.Id;
        }

        var candidates = await _userRepository.GetByRoleAsync(step.RequiredRole, departmentId: null, cancellationToken);
        var assignee = candidates.FirstOrDefault();

        if (assignee is null)
        {
            throw new NotFoundException($"User dengan Role '{step.RequiredRole}'", step.RequiredRole);
        }

        return assignee.Id;
    }
}