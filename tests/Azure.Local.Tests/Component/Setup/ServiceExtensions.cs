using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.ServiceBus;
using Azure.Local.Infrastructure.Timesheets;
using Azure.Local.Tests.Component.Timesheets.Fakes.Repositories;
using Azure.Local.Tests.Component.Timesheets.Fakes.ServiceBus;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Azure.Local.Tests.Component.Setup
{
    [ExcludeFromCodeCoverage]
    public static class ServiceExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection SetupBaseServices()
            {
                services.AddTimesheetPersistence();
                services.AddServiceBus();
                return services;
            }

            private IServiceCollection AddTimesheetPersistence()
            {
                // Remove existing registrations setup in Program.cs or Infrastructure
                services.RemoveAll<IRepository<TimesheetRepositoryItem>>();
                services.AddSingleton<IRepository<TimesheetRepositoryItem>, FakeRepository<TimesheetRepositoryItem>>();
                return services;
            }

            private IServiceCollection AddServiceBus()
            {
                // Remove existing registrations setup in Program.cs or Infrastructure
                services.RemoveAll<IServiceBusClient>();
                services.AddSingleton<IServiceBusClient, FakeServiceBusClient>();
                return services;
            }
        }
    }
}
