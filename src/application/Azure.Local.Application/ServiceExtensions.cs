using Azure.Local.Application.Timesheets;
using Azure.Local.Application.Timesheets.Workflows;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Local.Application
{
    public static class ServiceExtensions
    {
        extension(IServiceCollection services) 
        {
            public IServiceCollection AddApplication()
            {
                services.AddSingleton<ITimesheetWorkflow, TimesheetWorkflow>();
                services.AddSingleton<TimesheetApplication>();
                services.AddSingleton<ITimesheetApplication>(serviceProvider => serviceProvider.GetRequiredService<TimesheetApplication>());
                services.AddSingleton<ITimesheetApplicationV1>(serviceProvider => serviceProvider.GetRequiredService<TimesheetApplication>());
                return services;
            }
        }
    }
}
