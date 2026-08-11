using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Application.Abstractions.Persistence;

public interface IVendorApprovalTokenRepository
{
    Task<VendorApprovalToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task AddAsync(VendorApprovalToken entity, CancellationToken cancellationToken = default);
}