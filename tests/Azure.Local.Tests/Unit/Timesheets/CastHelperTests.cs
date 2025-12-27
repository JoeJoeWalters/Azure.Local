using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.ApiService.Timesheets.Helpers;

namespace Azure.Local.Tests.Unit.Timesheets
{
    public class CastHelperTests
    {
        [Fact]
        public void Cast_AddTimesheetHttpRequest_To_PatchTimesheetHttpRequest_ShouldMatch()
        {
            // ARRANGE
            AddTimesheetHttpRequest source = new()
            {
                Id = Guid.NewGuid().ToString(),
                From = DateTime.UtcNow,
                To = DateTime.UtcNow.AddDays(7),
                PersonId = Guid.NewGuid().ToString(),
                Components =
                    [
                        new TimesheetHttpRequestComponent()
                        {
                            Units = 8,
                            From = DateTime.UtcNow,
                            To = DateTime.UtcNow,
                            Code = "TEST"
                        }
                    ]
            };

            // ACT
            PatchTimesheetHttpRequest? result = source.ToPatchTimesheetHttpRequest();

            // ASSERT
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(source);
        }

        [Fact]
        public void Cast_Null_AddTimesheetHttpRequest_To_PatchTimesheetHttpRequest_ShouldBeNull()
        {
            // ARRANGE
            AddTimesheetHttpRequest? source = null;

            // ACT
            PatchTimesheetHttpRequest? result = source.ToPatchTimesheetHttpRequest();

            // ASSERT
            result.Should().BeNull();
        }

        [Fact]
        public void Cast_AddTimesheetHttpRequest_To_TimesheetItem_WithNullComponent_ShouldThrowError()
        {
            // ARRANGE
            AddTimesheetHttpRequest source = new()
            {
                Id = Guid.NewGuid().ToString(),
                From = DateTime.UtcNow,
                To = DateTime.UtcNow.AddDays(7),
                PersonId = Guid.NewGuid().ToString(),
                Components =
                    [
                        default!
                    ]
            };

            // ACT
            Action act = () => source.ToPatchTimesheetHttpRequest();

            // ASSERT
            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("resulted in null."));
        }
    }
}
