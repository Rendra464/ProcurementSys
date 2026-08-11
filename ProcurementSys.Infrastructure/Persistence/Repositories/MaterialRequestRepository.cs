using Microsoft.EntityFrameworkCore;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Infrastructure.Persistence.Repositories;

public class MaterialRequestRepository : IMaterialRequestRepository
{
    private readonly ApplicationDbContext _context;
    public MaterialRequestRepository(ApplicationDbContext context) => _context = context;

    public Task<MaterialRequest?> GetByIdAsync(int id, CancellationToken ct)
        => _context.MaterialRequests.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(MaterialRequest entity, CancellationToken ct)
        => await _context.MaterialRequests.AddAsync(entity, ct);

    public Task<List<MaterialRequest>> GetDashboardAsync(string? status, CancellationToken ct)
    {
        var query = _context.MaterialRequests.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(status)) query = query.Where(x => x.Status == status);
        return query.OrderByDescending(x => x.CreatedAt).ToListAsync(ct);
    }
}
