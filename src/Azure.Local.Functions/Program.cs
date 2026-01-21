using Azure.Local.Application.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Messaging;
using Azure.Local.Infrastructure.Timesheets;
using Azure.Local.Infrastructure.Timesheets.FileProcessing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

var host = new HostBuilder()
    //.ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Configure Cosmos DB Repository with Options pattern so it can be injected
        // and the test runner can override it if needed.
        services.AddOptions<CosmosRepositorySettings>()
            .Configure(x =>
            {
                x.ConnectionString = config["CosmosDb:ConnectionString"] ?? string.Empty;
                x.DatabaseId = config["CosmosDb:DatabaseId"] ?? string.Empty;
                x.ContainerId = config["CosmosDb:ContainerId"] ?? string.Empty;
            });

        // Configure Service Bus with Options pattern so it can be injected
        // and the test runner can override it if needed.
        services.AddOptions<ServiceBusSettings>()
            .Configure(x =>
            {
                x.ConnectionString = config["ServiceBus:ConnectionString"] ?? string.Empty;
                x.QueueName = config["ServiceBus:QueueName"] ?? string.Empty;
            });

        services.AddSingleton<IRepository<TimesheetRepositoryItem>, CosmosRepository<TimesheetRepositoryItem>>();
        services.AddSingleton<IServiceBusClient, AzureServiceBusClient>();

        services.AddSingleton<ITimesheetApplication, TimesheetApplication>();
        services.AddSingleton<ITimesheetFileProcessor, TimesheetFileProcessor>();
    })
    .Build();

host.Run();
