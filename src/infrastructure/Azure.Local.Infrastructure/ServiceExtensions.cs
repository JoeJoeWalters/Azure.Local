using Azure.Local.Domain;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Local.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<CosmosRepositorySettings>()
                .Configure(x => 
                    { 
                        x.ConnectionString = configuration["CosmosDb:ConnectionString"]; 
                        x.DatabaseId = configuration["CosmosDb:DatabaseId"];
                        x.ContainerId = configuration["CosmosDb:ContainerId"];
                    });
            services.AddSingleton<IRepository<RepositoryTestItem>, CosmosRepository<RepositoryTestItem>>();

            // Register infrastructure services here
            return services;
        }
    }
}
