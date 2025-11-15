using Azure.Local.ApiService.Tests.Component.Setup;
using Azure.Local.ApiService.Tests.Component.Timesheets.Fakes.Applications;
using Azure.Local.Application.Timesheets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Azure.Local.ApiService.Tests.Component.Timesheets.Setup
{
    public class ApiServiceWebApplicationFactoryExceptionHandling : WebApplicationFactory<Program>
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

                    services.RemoveAll<ITimesheetApplication>();
                    services.AddSingleton<ITimesheetApplication, FakeFailingTimesheetApplication>();
                });
        }
    }
}