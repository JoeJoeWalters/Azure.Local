using Azure.Local.Application.Timesheets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Local.Application
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Register application services here
            services.AddSingleton<ITimesheetApplication, TimesheetApplication>();

            return services;
        }
    }
}
