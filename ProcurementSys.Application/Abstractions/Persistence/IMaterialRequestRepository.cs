using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Application.Abstractions.Persistence;

public interface IMaterialRequestRepository
{
    Task<MaterialRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(MaterialRequest entity, CancellationToken cancellationToken = default);

}
