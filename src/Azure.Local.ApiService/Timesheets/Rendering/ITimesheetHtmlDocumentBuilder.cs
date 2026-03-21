using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public interface ITimesheetHtmlDocumentBuilder
    {
        string Build(TimesheetItem item);
    }
}
