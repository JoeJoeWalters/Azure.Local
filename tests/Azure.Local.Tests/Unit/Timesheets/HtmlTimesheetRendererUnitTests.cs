using Azure.Local.ApiService.Timesheets.Rendering;
using Azure.Local.Domain.Timesheets;
using Azure.Local.Tests.Unit.Timesheets.Helpers;
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

            var result = await sut.RenderAsync(item, TestContext.Current.CancellationToken);
            var html = Encoding.UTF8.GetString(result.Content);

            result.ContentType.Should().StartWith("text/html");
            html.Should().Contain("Timesheet ts-&lt;1&gt;");
            html.Should().Contain("person-&lt;1&gt;");
            html.Should().Contain("Components");
            TimesheetQrCodeTestHelper.Decode(html).Text.Should().Be(item.Id);
        }

        [Theory]
        [InlineData("f47ac10b-58cc-4372-a567-0e02b2c3d479")]
        [InlineData("ts-\"<script>&")]
        [InlineData("timesheet-\u00e9-\u65e5")]
        public async Task Render_ShouldIncludeScannableTimesheetId_InTopRightHeader(string id)
        {
            var sut = new HtmlTimesheetRenderer(new TimesheetHtmlDocumentBuilder());
            var item = new TimesheetItem
            {
                Id = id,
                PersonId = "person-1",
                From = DateTime.UtcNow.Date,
                To = DateTime.UtcNow.Date.AddDays(1),
                CreatedBy = "person-1"
            };

            var result = await sut.RenderAsync(item, TestContext.Current.CancellationToken);
            var html = Encoding.UTF8.GetString(result.Content);

            TimesheetQrCodeTestHelper.Decode(html).Text.Should().Be(id);
            html.Should().Contain("justify-content: space-between; align-items: flex-start");
            html.Should().Contain("width: 40mm; height: 40mm; flex: none");
            var headerStart = html.IndexOf("<header", StringComparison.Ordinal);
            var qrPosition = html.IndexOf("<img class=\"timesheet-qr\"", StringComparison.Ordinal);
            var headerEnd = html.IndexOf("</header>", StringComparison.Ordinal);
            qrPosition.Should().BeGreaterThan(headerStart).And.BeLessThan(headerEnd);
        }
    }
}
