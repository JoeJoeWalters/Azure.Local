using Azure.Local.ApiService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Azure.Local.Tests.Component.Setup
{
    public class ApiServiceWebApplicationFactoryBase : WebApplicationFactory<Program>
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
                    services.SetupBaseServices();
                });
        }
    }
}