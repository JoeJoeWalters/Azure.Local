namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public static class ServiceExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddTimesheetRendering()
            {
                services.AddSingleton<ITimesheetHtmlDocumentBuilder, TimesheetHtmlDocumentBuilder>();
                services.AddSingleton<IHtmlToPdfConverter, PlaywrightHtmlToPdfConverter>();
                services.AddSingleton<ITimesheetRenderer, HtmlTimesheetRenderer>();
                services.AddSingleton<ITimesheetRenderer, PdfTimesheetRenderer>();
                services.AddSingleton<ITimesheetRenderService, TimesheetRenderService>();
                return services;
            }
        }
    }
}
