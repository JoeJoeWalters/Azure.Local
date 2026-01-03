using Azure.Local.Application.Timesheets;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Local.Application
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)//, IConfiguration configuration)
        {
            services.AddTimesheetLogic();
            return services;
        }

        private static IServiceCollection AddTimesheetLogic(this IServiceCollection services)
        {
            services.AddSingleton<ITimesheetApplication, TimesheetApplication>();
            return services;
        }
    }
}
