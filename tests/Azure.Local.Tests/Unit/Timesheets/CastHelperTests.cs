using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.ApiService.Timesheets.Helpers;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
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
                CreatedBy = Guid.NewGuid().ToString(),
                ManagerId = Guid.NewGuid().ToString(),
                Comments = "Test comments",
                Components =
                    [
                        new TimesheetHttpRequestComponent()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Units = 8,
                            From = DateTime.UtcNow,
                            To = DateTime.UtcNow.AddHours(8),
                            TimeCode = "TEST",
                            ProjectCode = "PROJECT",
                            Description = "Test work",
                            WorkType = "Regular",
                            IsBillable = true
                        }
                    ]
            };

            // ACT
            PatchTimesheetHttpRequest? result = source.ToPatchTimesheetHttpRequest();

            // ASSERT
            result.Should().NotBeNull();
            result!.Id.Should().Be(source.Id);
            result.PersonId.Should().Be(source.PersonId);
            result.CreatedBy.Should().Be(source.CreatedBy);
            result.ManagerId.Should().Be(source.ManagerId);
            result.Comments.Should().Be(source.Comments);
            result.Components.Should().HaveCount(1);
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
                CreatedBy = Guid.NewGuid().ToString(),
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
