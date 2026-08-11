using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProcurementSys.Workers.Notification.Consumers;

IHostBuilder hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);

hostBuilder.ConfigureServices((hostContext, services) =>
{
    services.AddMassTransit(x =>
    {
        x.AddConsumer<ProcurementApprovalStatusChangedConsumer>();
        x.AddConsumer<VendorApprovalLinkGeneratedConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            var rabbitHost = hostContext.Configuration["RabbitMq:Host"] ?? "localhost";
            cfg.Host(rabbitHost, "/");

            cfg.ConfigureEndpoints(context);
        });
    });
});

IHost app = hostBuilder.Build();
await app.RunAsync();