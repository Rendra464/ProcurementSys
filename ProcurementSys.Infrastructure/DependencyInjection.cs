using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProcurementSys.Application.Abstractions.Messaging;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Application.Features.MaterialRequests.Services;
using ProcurementSys.Application.Features.ProcurementRequests.Services;
using ProcurementSys.Application.Features.Workflow.Services;
using ProcurementSys.Infrastructure.Messaging;
using ProcurementSys.Infrastructure.Persistence;
using ProcurementSys.Infrastructure.Persistence.Repositories;

namespace ProcurementSys.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IMaterialRequestRepository, MaterialRequestRepository>();
        services.AddScoped<IProcurementRequestRepository, ProcurementRequestRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();

        services.AddScoped<IIntegrationEventPublisher, RabbitMqIntegrationEventPublisher>();

        // Service dari Application layer, implementasinya di Application tapi registrasi DI-nya di sini
        // supaya konsisten satu tempat "wiring" per layer.
        services.AddScoped<IMaterialRequestService, MaterialRequestService>();
        services.AddScoped<IProcurementRequestService, ProcurementRequestService>();
        services.AddScoped<IApprovalWorkflowService, ApprovalWorkflowService>();

        // TODO: services.AddMassTransit(x => { x.UsingRabbitMq((ctx, cfg) => cfg.Host(configuration["RabbitMq:Host"])); });
        // TODO: services.AddHangfire(...).AddHangfireServer();

        return services;
    }
}
