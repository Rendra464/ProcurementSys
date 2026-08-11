using ProcurementSys.Domain.Entities;
using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<User>> GetByRoleAsync(UserRole role, int? departmentId = null, CancellationToken cancellationToken = default);
    Task<User?> GetByVendorIdAsync(int vendorId, CancellationToken cancellationToken = default);
}