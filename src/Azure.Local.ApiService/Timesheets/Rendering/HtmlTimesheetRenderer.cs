using Azure.Local.Domain.Timesheets;
using System.Text;

namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public sealed class HtmlTimesheetRenderer(ITimesheetHtmlDocumentBuilder htmlDocumentBuilder) : ITimesheetRenderer
    {
        public TimesheetRenderOutputType OutputType => TimesheetRenderOutputType.Html;

        public Task<TimesheetRenderResult> RenderAsync(TimesheetItem item, CancellationToken cancellationToken = default)
        {
            var html = htmlDocumentBuilder.Build(item);
            var result = new TimesheetRenderResult
            {
                ContentType = "text/html; charset=utf-8",
                Content = Encoding.UTF8.GetBytes(html)
            };
            return Task.FromResult(result);
        }
    }
}
