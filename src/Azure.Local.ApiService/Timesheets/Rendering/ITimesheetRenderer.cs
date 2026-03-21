using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public interface ITimesheetRenderer
    {
        TimesheetRenderOutputType OutputType { get; }
        TimesheetRenderResult Render(TimesheetItem item);
    }
}
