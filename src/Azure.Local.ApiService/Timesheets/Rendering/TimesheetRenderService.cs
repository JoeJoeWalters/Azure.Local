using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public sealed class TimesheetRenderService(IEnumerable<ITimesheetRenderer> renderers) : ITimesheetRenderService
    {
        private readonly IReadOnlyDictionary<TimesheetRenderOutputType, ITimesheetRenderer> _renderers = renderers.ToDictionary(
            renderer => renderer.OutputType,
            renderer => renderer);

        public async Task<TimesheetRenderResult> RenderAsync(TimesheetItem item, TimesheetRenderOutputType outputType, CancellationToken cancellationToken = default)
        {
            if (!_renderers.TryGetValue(outputType, out var renderer))
            {
                throw new NotSupportedException($"Timesheet output type '{outputType}' is not supported.");
            }

            return await renderer.RenderAsync(item, cancellationToken);
        }
    }
}
