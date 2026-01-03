using Azure.Local.Application.Timesheets;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Local.Application
{
    public static class ServiceExtensions
    {
        extension(IServiceCollection services) 
        {
            public IServiceCollection AddApplication()
            {
                services.AddTimesheetLogic();
                return services;
            }

            private IServiceCollection AddTimesheetLogic()
            {
                services.AddSingleton<ITimesheetApplication, TimesheetApplication>();
                return services;
            }
        }
    }
}
