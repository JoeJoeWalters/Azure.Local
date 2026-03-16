using Azure.Local.Application;
using Azure.Local.Application.Timesheets;
using Azure.Local.Application.Timesheets.FileProcessing;
using Azure.Local.Infrastructure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddApplication();
        services.AddInfrastructure(context.Configuration);
    })
    .Build();

host.Run();