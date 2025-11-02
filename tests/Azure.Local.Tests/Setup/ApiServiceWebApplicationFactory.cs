using Azure.Local.ApiService.Tests.Component.Fakes.Repositories;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Azure.Local.ApiService.Tests.Component.Setup
{
    public class ApiServiceWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // You can customize the web host configuration here if needed
            base.ConfigureWebHost(builder);

            builder
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    // Additional Configuration Setup
                }).ConfigureTestServices(services =>
                {
                    // Additional Test Services Setup
                    IRepository<RepositoryTestItem> repository = new FakeRepository<RepositoryTestItem>();
                    services.AddSingleton<IRepository<RepositoryTestItem>>(repository);
                });
        }
    }
}
