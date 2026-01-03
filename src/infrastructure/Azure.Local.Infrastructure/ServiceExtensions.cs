using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Timesheets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Azure.Local.Infrastructure
{
    [ExcludeFromCodeCoverage]

    public static class ServiceExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddInfrastructure(IConfiguration configuration)
            {
                // Configure Cosmos DB Repository with Options pattern so it can be injected
                // and the test runner can override it if needed.
                services.AddOptions<CosmosRepositorySettings>()
                    .Configure(x =>
                        {
                            x.ConnectionString = configuration["CosmosDb:ConnectionString"] ?? string.Empty;
                            x.DatabaseId = configuration["CosmosDb:DatabaseId"] ?? string.Empty;
                            x.ContainerId = configuration["CosmosDb:ContainerId"] ?? string.Empty;
                        });
                services.AddTimesheetPersistence();

                return services;
            }

            private IServiceCollection AddTimesheetPersistence()
            {
                services.AddSingleton<IRepository<TimesheetRepositoryItem>, CosmosRepository<TimesheetRepositoryItem>>();
                return services;
            }
        }
    }
}
