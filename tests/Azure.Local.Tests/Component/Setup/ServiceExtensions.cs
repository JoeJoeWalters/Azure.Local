using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Timesheets;
using Azure.Local.Tests.Component.Timesheets.Fakes.Repositories;
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
                return services;
            }

            private IServiceCollection AddTimesheetPersistence()
            {
                // Remove existing registrations setup in Program.cs or Infrastructure
                services.RemoveAll<IRepository<TimesheetRepositoryItem>>();
                services.AddSingleton<IRepository<TimesheetRepositoryItem>, FakeRepository<TimesheetRepositoryItem>>();
                return services;
            }
        }
    }
}
