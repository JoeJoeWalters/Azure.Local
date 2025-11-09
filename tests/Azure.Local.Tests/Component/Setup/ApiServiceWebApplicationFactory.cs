using Azure.Local.ApiService.Tests.Component.Fakes.Repositories;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Test;
using Azure.Local.Infrastructure.Timesheets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
                    // Remove existing registrations setup in Program.cs or Infrastructure
                    services.RemoveAll<IRepository<TimesheetRepositoryItem>>();

                    // Additional Test Services Setup
                    services.AddSingleton<IRepository<TimesheetRepositoryItem>, FakeRepository<TimesheetRepositoryItem>>();
                });
        }
    }
}
