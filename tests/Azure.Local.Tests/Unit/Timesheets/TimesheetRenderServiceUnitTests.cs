using Azure.Local.ApiService.Timesheets.Rendering;
using Azure.Local.Domain.Timesheets;
using System.Text;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class TimesheetRenderServiceUnitTests
    {
        [Fact]
        public async Task Render_ShouldUseRenderer_ForRequestedOutputType()
        {
            var item = CreateItem();
            var expected = Encoding.UTF8.GetBytes("<html>ok</html>");
            var renderer = new StubRenderer(TimesheetRenderOutputType.Html, expected);
            var sut = new TimesheetRenderService([renderer]);

            var result = await sut.RenderAsync(item, TimesheetRenderOutputType.Html);

            result.Content.Should().BeEquivalentTo(expected);
            result.ContentType.Should().Be("text/html; charset=utf-8");
        }

        [Fact]
        public async Task Render_ShouldThrow_WhenOutputTypeNotRegistered()
        {
            var item = CreateItem();
            var renderer = new StubRenderer(TimesheetRenderOutputType.Html, []);
            var sut = new TimesheetRenderService([renderer]);

            var action = async () => await sut.RenderAsync(item, (TimesheetRenderOutputType)999);
            await action.Should().ThrowAsync<NotSupportedException>();
        }

        private static TimesheetItem CreateItem()
            => new()
            {
                Id = "ts-1",
                PersonId = "person-1",
                From = DateTime.UtcNow.Date,
                To = DateTime.UtcNow.Date.AddDays(1),
                CreatedBy = "person-1"
            };

        private sealed class StubRenderer(TimesheetRenderOutputType outputType, byte[] payload) : ITimesheetRenderer
        {
            public TimesheetRenderOutputType OutputType => outputType;

            public Task<TimesheetRenderResult> RenderAsync(TimesheetItem item, CancellationToken cancellationToken = default)
                => Task.FromResult(new TimesheetRenderResult
                {
                    ContentType = "text/html; charset=utf-8",
                    Content = payload
                });
        }
    }
}
