using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public interface ITimesheetRenderService
    {
        Task<TimesheetRenderResult> RenderAsync(TimesheetItem item, TimesheetRenderOutputType outputType, CancellationToken cancellationToken = default);
    }
}
