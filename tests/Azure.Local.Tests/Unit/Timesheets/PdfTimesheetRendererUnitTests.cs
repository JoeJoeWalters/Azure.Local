using Azure.Local.ApiService.Timesheets.Rendering;
using Azure.Local.Domain.Timesheets;
using System.Text;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class PdfTimesheetRendererUnitTests
    {
        [Fact]
        public async Task RenderAsync_ShouldReturnPdfPayload()
        {
            var htmlBuilder = new TimesheetHtmlDocumentBuilder();
            var converter = new StubConverter();
            var sut = new PdfTimesheetRenderer(htmlBuilder, converter);
            var item = new TimesheetItem
            {
                Id = "ts-1",
                PersonId = "person-1",
                From = DateTime.UtcNow,
                To = DateTime.UtcNow.AddDays(1),
                CreatedBy = "person-1"
            };

            var result = await sut.RenderAsync(item);
            var payloadText = Encoding.UTF8.GetString(result.Content);

            result.ContentType.Should().Be("application/pdf");
            result.FileDownloadName.Should().Be("ts-1.pdf");
            payloadText.Should().Contain("Timesheet ts-1");
        }

        private sealed class StubConverter : IHtmlToPdfConverter
        {
            public Task<byte[]> ConvertAsync(string html, CancellationToken cancellationToken = default)
                => Task.FromResult(Encoding.UTF8.GetBytes(html));
        }
    }
}
