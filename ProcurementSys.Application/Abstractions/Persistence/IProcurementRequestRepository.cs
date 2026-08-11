using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Application.Abstractions.Persistence;

public interface IProcurementRequestRepository
{
    Task<ProcurementRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(ProcurementRequest entity, CancellationToken cancellationToken = default);
    Task<ProcurementRequest?> GetByMaterialRequestIdAsync(int materialRequestId, CancellationToken cancellationToken = default);
}
