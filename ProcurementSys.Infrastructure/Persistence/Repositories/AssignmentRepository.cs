using Microsoft.EntityFrameworkCore;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Infrastructure.Persistence.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly ApplicationDbContext _context;
    public AssignmentRepository(ApplicationDbContext context) => _context = context;

    public Task<List<Assignment>> GetActiveByEntityAsync(string entityType, int entityId, CancellationToken ct)
        => _context.Assignments
            .Where(a => a.EntityType == entityType && a.EntityId == entityId && a.Status == "Pending")
            .ToListAsync(ct);

    public async Task AddAsync(Assignment assignment, CancellationToken ct)
        => await _context.Assignments.AddAsync(assignment, ct);

    public Task<Assignment?> GetByIdAsync(int id, CancellationToken ct)
        => _context.Assignments.FirstOrDefaultAsync(a => a.Id == id, ct);
}
