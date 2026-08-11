using ProcurementSys.Domain.Entities;
using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Application.Abstractions.Persistence;

public interface IAssignmentRepository
{
    Task<Assignment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Assignment entity, CancellationToken cancellationToken = default);
    Task<Assignment?> GetPendingByEntityAsync(WorkflowProcessType entityType, int entityId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Assignment>> GetPendingByAssigneeUserIdAsync(int userId, CancellationToken cancellationToken = default);
}