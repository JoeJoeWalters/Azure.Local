namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public static class ServiceExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddTimesheetRendering()
            {
                services.AddSingleton<ITimesheetRenderer, HtmlTimesheetRenderer>();
                services.AddSingleton<ITimesheetRenderService, TimesheetRenderService>();
                return services;
            }
        }
    }
}
