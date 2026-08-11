using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProcurementSys.Application.Common.Behaviors;

namespace ProcurementSys.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // Feature services didaftarkan di sini juga, contoh:
        // services.AddScoped<IMaterialRequestService, MaterialRequestService>();
        // services.AddScoped<IProcurementRequestService, ProcurementRequestService>();
        // services.AddScoped<IApprovalWorkflowService, ApprovalWorkflowService>();

        return services;
    }
}
