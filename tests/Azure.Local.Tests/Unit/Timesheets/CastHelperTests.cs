using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.ApiService.Timesheets.Contracts.V1;
using Azure.Local.ApiService.Timesheets.Helpers;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class CastHelperTests
    {
        [Fact]
        public void Cast_AddTimesheetHttpRequest_To_PatchTimesheetHttpRequest_ShouldMatch()
        {
            var source = CreateAddRequest();

            var result = source.ToPatchTimesheetHttpRequest();

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
            AddTimesheetHttpRequestV1? source = null;

            var result = source.ToPatchTimesheetHttpRequest();

            result.Should().BeNull();
        }

        [Fact]
        public void Cast_AddTimesheetHttpRequest_To_TimesheetItem_ShouldMapFields()
        {
            var source = CreateAddRequest();
            var before = DateTime.UtcNow;

            var result = source.ToTimesheetItem();

            var after = DateTime.UtcNow;
            result.Id.Should().Be(source.Id);
            result.PersonId.Should().Be(source.PersonId);
            result.CreatedBy.Should().Be(source.CreatedBy);
            result.ManagerId.Should().Be(source.ManagerId);
            result.Comments.Should().Be(source.Comments);
            result.Status.Should().Be(TimesheetStatus.Draft);
            result.Components.Should().ContainSingle();
            result.CreatedDate.Should().BeOnOrAfter(before);
            result.CreatedDate.Should().BeOnOrBefore(after);
            result.ModifiedDate.Should().BeOnOrAfter(before);
            result.ModifiedDate.Should().BeOnOrBefore(after);
        }

        [Fact]
        public void Cast_TimesheetHttpRequestComponent_To_TimesheetComponentItem_UsesDefaults()
        {
            var component = new TimesheetHttpRequestComponentV1
            {
                Id = null,
                Units = 7.5,
                From = DateTime.UtcNow.AddHours(-8),
                To = DateTime.UtcNow,
                TimeCode = "DEV",
                ProjectCode = "PRJ",
                WorkType = "   ",
                IsBillable = null
            };

            var result = component.ToTimesheetComponentItem();

            result.Id.Should().NotBeNullOrWhiteSpace();
            result.WorkType.Should().Be(WorkType.Regular);
            result.IsBillable.Should().BeTrue();
        }

        [Fact]
        public void Cast_TimesheetHttpRequestComponent_To_TimesheetComponentItem_ShouldParseValidWorkType()
        {
            var component = new TimesheetHttpRequestComponentV1
            {
                Id = "component-id",
                Units = 8,
                From = DateTime.UtcNow.AddHours(-8),
                To = DateTime.UtcNow,
                TimeCode = "DEV",
                ProjectCode = "PRJ",
                WorkType = "Overtime",
                IsBillable = false
            };

            var result = component.ToTimesheetComponentItem();

            result.Id.Should().Be("component-id");
            result.WorkType.Should().Be(WorkType.Overtime);
            result.IsBillable.Should().BeFalse();
        }

        [Fact]
        public void Cast_TimesheetHttpRequestComponent_To_TimesheetComponentItem_ShouldFallbackForInvalidWorkType()
        {
            var component = new TimesheetHttpRequestComponentV1
            {
                Id = "component-id",
                Units = 8,
                From = DateTime.UtcNow.AddHours(-8),
                To = DateTime.UtcNow,
                TimeCode = "DEV",
                ProjectCode = "PRJ",
                WorkType = "invalid-value",
                IsBillable = true
            };

            var result = component.ToTimesheetComponentItem();

            result.WorkType.Should().Be(WorkType.Regular);
        }

        [Fact]
        public void Cast_TimesheetItem_To_TimesheetResponse_ShouldMapFieldsAndCapabilities()
        {
            var from = new DateTime(2026, 2, 1, 9, 0, 0, DateTimeKind.Utc);
            var to = new DateTime(2026, 2, 1, 17, 0, 0, DateTimeKind.Utc);
            var item = new TimesheetItem
            {
                Id = "ts-1",
                PersonId = "person-1",
                From = from,
                To = to,
                CreatedBy = "person-1",
                Status = TimesheetStatus.Submitted,
                SubmittedDate = from,
                ApprovedBy = "manager-1",
                ManagerId = "manager-1",
                Comments = "note",
                ETag = "etag-1",
                Version = 2,
                Components =
                [
                    new TimesheetComponentItem
                    {
                        Id = "c-1",
                        Units = 8,
                        From = from,
                        To = to,
                        TimeCode = "DEV",
                        ProjectCode = "PRJ",
                        WorkType = WorkType.Holiday,
                        IsBillable = false,
                        IsLocked = true
                    }
                ]
            };

            var result = item.ToTimesheetResponse();

            result.Id.Should().Be(item.Id);
            result.PersonId.Should().Be(item.PersonId);
            result.Status.Should().Be(nameof(TimesheetStatus.Submitted));
            result.Components.Should().ContainSingle();
            result.Components.Single().WorkType.Should().Be(nameof(WorkType.Holiday));
            result.Components.Single().IsLocked.Should().BeTrue();
            result.TotalUnits.Should().Be(8);
            result.CanEdit.Should().BeFalse();
            result.CanSubmit.Should().BeFalse();
            result.CanApprove.Should().BeTrue();
            result.CanReject.Should().BeTrue();
            result.CanRecall.Should().BeTrue();
            result.ETag.Should().Be("etag-1");
            result.Version.Should().Be(2);
        }

        [Fact]
        public void Cast_AddTimesheetHttpRequest_To_PatchTimesheetHttpRequest_WithNullComponent_ShouldThrowError()
        {
            var source = new AddTimesheetHttpRequestV1
            {
                Id = Guid.NewGuid().ToString(),
                From = DateTime.UtcNow,
                To = DateTime.UtcNow.AddDays(7),
                PersonId = Guid.NewGuid().ToString(),
                CreatedBy = Guid.NewGuid().ToString(),
                Components = [default!]
            };

            Action act = () => source.ToPatchTimesheetHttpRequest();

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("resulted in null."));
        }

        private static AddTimesheetHttpRequestV1 CreateAddRequest()
            => new()
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
                    new TimesheetHttpRequestComponentV1
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
    }
}
