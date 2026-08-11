using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Application.Abstractions.Persistence;

public interface IVendorRepository
{
    Task<Vendor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}