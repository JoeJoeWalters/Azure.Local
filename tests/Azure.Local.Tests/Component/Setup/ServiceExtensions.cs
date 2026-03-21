using Azure.Local.Application.Timesheets;
using Azure.Local.Application.Timesheets.FileProcessing;
using Azure.Local.ApiService.Timesheets.Rendering;
using Azure.Local.Infrastructure.Messaging;
using Azure.Local.Tests.Component.Timesheets.Fakes.Repositories;
using Azure.Local.Tests.Component.Timesheets.Fakes.Rendering;
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
                services.AddRenderingSupport();
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

            private IServiceCollection AddRenderingSupport()
            {
                services.RemoveAll<IHtmlToPdfConverter>();
                services.AddSingleton<IHtmlToPdfConverter, FakeHtmlToPdfConverter>();
                return services;
            }
        }
    }
}
