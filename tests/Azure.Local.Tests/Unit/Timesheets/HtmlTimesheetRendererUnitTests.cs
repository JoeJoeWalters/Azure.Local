using Azure.Local.ApiService.Timesheets.Rendering;
using Azure.Local.Domain.Timesheets;
using System.Text;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class HtmlTimesheetRendererUnitTests
    {
        [Fact]
        public async Task Render_ShouldReturnHtml_WithEncodedContent()
        {
            var sut = new HtmlTimesheetRenderer(new TimesheetHtmlDocumentBuilder());
            var item = new TimesheetItem
            {
                Id = "ts-<1>",
                PersonId = "person-<1>",
                From = DateTime.UtcNow.Date,
                To = DateTime.UtcNow.Date.AddDays(1),
                CreatedBy = "creator",
                Components =
                [
                    new TimesheetComponentItem
                    {
                        Id = "comp-1",
                        Units = 8,
                        From = DateTime.UtcNow.Date,
                        To = DateTime.UtcNow.Date.AddHours(8),
                        TimeCode = "DEV",
                        ProjectCode = "PRJ",
                        WorkType = WorkType.Regular,
                        IsBillable = true
                    }
                ]
            };

            var result = await sut.RenderAsync(item);
            var html = Encoding.UTF8.GetString(result.Content);

            result.ContentType.Should().StartWith("text/html");
            html.Should().Contain("Timesheet ts-&lt;1&gt;");
            html.Should().Contain("person-&lt;1&gt;");
            html.Should().Contain("Components");
        }
    }
}
