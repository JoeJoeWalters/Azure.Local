using Azure.Local.Domain;
using Azure.Local.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Local.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            IRepository<RepositoryTestItem> repository = new CosmosRepository<RepositoryTestItem>();
            services.AddSingleton<IRepository<RepositoryTestItem>>(repository);

            // Register infrastructure services here
            return services;
        }
    }
}
