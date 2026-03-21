using Azure.Local.ApiService.Timesheets.Rendering;
using Azure.Local.Domain.Timesheets;
using System.Text;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class TimesheetRenderServiceUnitTests
    {
        [Fact]
        public void Render_ShouldUseRenderer_ForRequestedOutputType()
        {
            var item = CreateItem();
            var expected = Encoding.UTF8.GetBytes("<html>ok</html>");
            var renderer = new StubRenderer(TimesheetRenderOutputType.Html, expected);
            var sut = new TimesheetRenderService([renderer]);

            var result = sut.Render(item, TimesheetRenderOutputType.Html);

            result.Content.Should().BeEquivalentTo(expected);
            result.ContentType.Should().Be("text/html; charset=utf-8");
        }

        [Fact]
        public void Render_ShouldThrow_WhenOutputTypeNotRegistered()
        {
            var item = CreateItem();
            var renderer = new StubRenderer(TimesheetRenderOutputType.Html, []);
            var sut = new TimesheetRenderService([renderer]);

            var action = () => sut.Render(item, (TimesheetRenderOutputType)999);
            action.Should().Throw<NotSupportedException>();
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

            public TimesheetRenderResult Render(TimesheetItem item)
                => new()
                {
                    ContentType = "text/html; charset=utf-8",
                    Content = payload
                };
        }
    }
}
