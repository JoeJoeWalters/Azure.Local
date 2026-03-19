using Azure.Local.ApiService;
using Azure.Local.Application.Timesheets;
using Azure.Local.Tests.Component.Setup;
using Azure.Local.Tests.Component.Timesheets.Fakes.Applications;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Azure.Local.Tests.Component.Timesheets.Setup
{
    [ExcludeFromCodeCoverage]
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
                }).ConfigureTestServices((Action<IServiceCollection>)(services =>
                {
                    // Remove existing registrations setup in Program.cs or Infrastructure
                    services.SetupBaseServices();

                    ServiceCollectionDescriptorExtensions.RemoveAll<ITimesheetApplication>(services);
                    ServiceCollectionDescriptorExtensions.RemoveAll<ITimesheetApplication>(services);
                    services.AddSingleton<FakeFailingTimesheetApplication>();
                    ServiceCollectionServiceExtensions.AddSingleton<ITimesheetApplication>(services, (Func<IServiceProvider, ITimesheetApplication>)(serviceProvider => serviceProvider.GetRequiredService<FakeFailingTimesheetApplication>()));
                    ServiceCollectionServiceExtensions.AddSingleton<ITimesheetApplication>(services, (Func<IServiceProvider, ITimesheetApplication>)(serviceProvider => serviceProvider.GetRequiredService<FakeFailingTimesheetApplication>()));
                }));
        }
    }
}
