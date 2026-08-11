using ProcurementSys.Domain.Entities;
using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Application.Abstractions.Persistence;

public interface IWorkflowDefinitionRepository
{
    Task<WorkflowDefinition?> GetActiveDefinitionAsync(WorkflowProcessType entityType, CancellationToken cancellationToken = default);
}