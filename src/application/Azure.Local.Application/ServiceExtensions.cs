using Azure.Local.Application.Timesheets;
using Azure.Local.Application.Timesheets.V1;
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
                services.AddSingleton<TimesheetApplicationV1>();
                services.AddSingleton<ITimesheetApplicationV1>(serviceProvider => serviceProvider.GetRequiredService<TimesheetApplicationV1>());
                return services;
            }
        }
    }
}
