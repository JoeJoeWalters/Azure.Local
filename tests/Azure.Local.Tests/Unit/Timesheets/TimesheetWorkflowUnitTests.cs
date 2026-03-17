using Azure.Local.Application.Timesheets.Workflows;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class TimesheetWorkflowUnitTests
    {
        private readonly TimesheetWorkflow _sut = new();

        [Fact]
        public void Submit_ShouldFail_WhenTimesheetCannotBeSubmitted()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Submitted);

            var result = _sut.Submit(timesheet, "person-1");

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle();
        }

        [Fact]
        public void Submit_ShouldFail_WhenBusinessRulesInvalid()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Draft);
            timesheet.Components.Single().Units = 0;

            var result = _sut.Submit(timesheet, "person-1");

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void Submit_ShouldSucceed_ForValidDraft()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Draft);

            var result = _sut.Submit(timesheet, "person-1");

            result.IsSuccess.Should().BeTrue();
            timesheet.Status.Should().Be(TimesheetStatus.Submitted);
            timesheet.SubmittedDate.Should().NotBeNull();
            timesheet.ModifiedBy.Should().Be("person-1");
        }

        [Fact]
        public void Approve_ShouldFail_WhenCannotApprove()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Draft);

            var result = _sut.Approve(timesheet, "manager-1");

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Approve_ShouldFail_WhenApproverIsSubmitter()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Submitted, personId: "person-1");

            var result = _sut.Approve(timesheet, "person-1");

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("You cannot approve your own timesheet.");
        }

        [Fact]
        public void Approve_ShouldSucceed_AndLockComponents()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Submitted, personId: "person-1");

            var result = _sut.Approve(timesheet, "manager-1");

            result.IsSuccess.Should().BeTrue();
            timesheet.Status.Should().Be(TimesheetStatus.Approved);
            timesheet.Components.Should().OnlyContain(c => c.IsLocked);
        }

        [Fact]
        public void Reject_ShouldFail_WhenCannotReject()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Draft);

            var result = _sut.Reject(timesheet, "manager-1", "reason");

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Reject_ShouldFail_WhenReasonMissing()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Submitted);

            var result = _sut.Reject(timesheet, "manager-1", " ");

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("Rejection reason is required.");
        }

        [Fact]
        public void Reject_ShouldSucceed_WithReason()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Submitted);

            var result = _sut.Reject(timesheet, "manager-1", "invalid hours");

            result.IsSuccess.Should().BeTrue();
            timesheet.Status.Should().Be(TimesheetStatus.Rejected);
            timesheet.RejectionReason.Should().Be("invalid hours");
        }

        [Fact]
        public void Recall_ShouldFail_WhenCannotRecall()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Draft);

            var result = _sut.Recall(timesheet, "person-1");

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Recall_ShouldFail_WhenUserIsNotOwnerOrCreator()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Submitted, personId: "person-1", createdBy: "creator-1");

            var result = _sut.Recall(timesheet, "other-user");

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("Only the timesheet owner can recall it.");
        }

        [Fact]
        public void Recall_ShouldSucceed_WhenCalledByOwner()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Submitted, personId: "person-1");

            var result = _sut.Recall(timesheet, "person-1");

            result.IsSuccess.Should().BeTrue();
            timesheet.Status.Should().Be(TimesheetStatus.Recalled);
        }

        [Fact]
        public void Recall_ShouldSucceed_WhenCalledByCreator()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Submitted, personId: "person-1", createdBy: "creator-1");

            var result = _sut.Recall(timesheet, "creator-1");

            result.IsSuccess.Should().BeTrue();
            timesheet.Status.Should().Be(TimesheetStatus.Recalled);
        }

        [Fact]
        public void Reopen_ShouldFail_WhenStatusIsNotRecalledOrRejected()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Draft);

            var result = _sut.Reopen(timesheet, "person-1");

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Reopen_ShouldSucceed_FromRecalledAndUnlockComponents()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Recalled);
            timesheet.Components.ForEach(c => c.IsLocked = true);

            var result = _sut.Reopen(timesheet, "person-1");

            result.IsSuccess.Should().BeTrue();
            timesheet.Status.Should().Be(TimesheetStatus.Draft);
            timesheet.Components.Should().OnlyContain(c => !c.IsLocked);
        }

        [Fact]
        public void Reopen_ShouldSucceed_FromRejected()
        {
            var timesheet = CreateTimesheet(TimesheetStatus.Rejected);
            timesheet.Components.ForEach(c => c.IsLocked = true);

            var result = _sut.Reopen(timesheet, "person-1");

            result.IsSuccess.Should().BeTrue();
            timesheet.Status.Should().Be(TimesheetStatus.Draft);
        }

        private static TimesheetItem CreateTimesheet(TimesheetStatus status, string personId = "person-1", string? createdBy = null)
        {
            var from = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc);
            var to = new DateTime(2026, 1, 11, 0, 0, 0, DateTimeKind.Utc);

            return new TimesheetItem
            {
                PersonId = personId,
                CreatedBy = createdBy ?? personId,
                From = from,
                To = to,
                Status = status,
                Components =
                [
                    new TimesheetComponentItem
                    {
                        Id = "comp-1",
                        Units = 8,
                        From = from.AddHours(9),
                        To = from.AddHours(17),
                        TimeCode = "DEV",
                        ProjectCode = "PRJ",
                        IsBillable = true
                    }
                ]
            };
        }
    }
}
