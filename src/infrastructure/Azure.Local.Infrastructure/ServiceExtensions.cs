using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Timesheets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Local.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
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
            services.AddSingleton<IRepository<TimesheetRepositoryItem>, CosmosRepository<TimesheetRepositoryItem>>();

            // Register infrastructure services here
            return services;
        }
    }
}
