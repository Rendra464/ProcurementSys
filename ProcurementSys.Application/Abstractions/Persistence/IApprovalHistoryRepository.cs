using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Application.Abstractions.Persistence;

public interface IApprovalHistoryRepository
{
    Task AddAsync(ApprovalHistory entity, CancellationToken cancellationToken = default);
}