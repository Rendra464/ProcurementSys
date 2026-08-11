using MediatR;
using Microsoft.EntityFrameworkCore;
using ProcurementSys.Domain.Common;
using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly IPublisher _publisher; // dispatch domain events setelah SaveChanges sukses

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher) : base(options)
        => _publisher = publisher;

    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<MaterialRequest> MaterialRequests => Set<MaterialRequest>();
    public DbSet<MaterialRequestItem> MaterialRequestItems => Set<MaterialRequestItem>();
    public DbSet<ProcurementRequest> ProcurementRequests => Set<ProcurementRequest>();
    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();
    public DbSet<WorkflowStep> WorkflowSteps => Set<WorkflowStep>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<ApprovalHistory> ApprovalHistories => Set<ApprovalHistory>();
    public DbSet<VendorApprovalToken> VendorApprovalTokens => Set<VendorApprovalToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        builder.Entity<Assignment>().Property(a => a.RowVersion).IsRowVersion();
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Kumpulkan domain event dari semua entity yang di-track sebelum SaveChanges, dispatch setelah sukses.
        var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count != 0)
            .ToList();

        var result = await base.SaveChangesAsync(ct);

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToList();
            entity.ClearDomainEvents();
            foreach (var domainEvent in events)
                await _publisher.Publish(domainEvent, ct);
        }

        return result;
    }
}
