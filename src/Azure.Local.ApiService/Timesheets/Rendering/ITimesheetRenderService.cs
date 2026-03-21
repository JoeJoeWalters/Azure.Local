using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public interface ITimesheetRenderService
    {
        TimesheetRenderResult Render(TimesheetItem item, TimesheetRenderOutputType outputType);
    }
}
