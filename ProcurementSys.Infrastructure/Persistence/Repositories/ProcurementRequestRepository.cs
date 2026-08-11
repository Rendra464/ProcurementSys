using Microsoft.EntityFrameworkCore;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Infrastructure.Persistence.Repositories;

public class ProcurementRequestRepository : IProcurementRequestRepository
{
    private readonly ApplicationDbContext _context;
    public ProcurementRequestRepository(ApplicationDbContext context) => _context = context;

    public Task<ProcurementRequest?> GetByIdAsync(int id, CancellationToken ct)
        => _context.ProcurementRequests.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(ProcurementRequest entity, CancellationToken ct)
        => await _context.ProcurementRequests.AddAsync(entity, ct);

    public Task<List<ProcurementRequest>> GetDashboardAsync(string? type, CancellationToken ct)
    {
        var query = _context.ProcurementRequests.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(type)) query = query.Where(x => x.Type == type);
        return query.OrderByDescending(x => x.CreatedAt).ToListAsync(ct);
    }
}
