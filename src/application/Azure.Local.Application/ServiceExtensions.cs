using Azure.Local.Application.Timesheets;
using Azure.Local.Infrastructure.Timesheets.FileProcessing;
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
                services.AddSingleton<ITimesheetFileProcessor, TimesheetFileProcessor>();
                return services;
            }
        }
    }
}
