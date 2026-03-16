using Azure.Local.Application.Timesheets;
using Azure.Local.Application.Timesheets.FileProcessing;
using Azure.Local.Infrastructure.Messaging;
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
                services.RemoveAll<ITimesheetRepository>();
                services.AddSingleton<ITimesheetRepository, FakeTimesheetRepository>();
                return services;
            }

            private IServiceCollection AddServiceBus()
            {
                services.RemoveAll<IServiceBusClient>();
                services.AddSingleton<IServiceBusClient, FakeServiceBusClient>();
                return services;
            }
        }
    }
}
