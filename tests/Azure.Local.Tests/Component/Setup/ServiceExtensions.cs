using Azure.Local.ApiService.Tests.Component.Timesheets.Fakes.Repositories;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Timesheets;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Azure.Local.ApiService.Tests.Component.Setup
{
    public static class ServiceExtensions
    {
        public static IServiceCollection SetupBaseServices(this IServiceCollection services)
        {
            // Remove existing registrations setup in Program.cs or Infrastructure
            services.RemoveAll<IRepository<TimesheetRepositoryItem>>();

            // Additional Test Services Setup
            services.AddSingleton<IRepository<TimesheetRepositoryItem>, FakeRepository<TimesheetRepositoryItem>>();

            return services;
        }
    }
}
