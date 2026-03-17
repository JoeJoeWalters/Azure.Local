using Azure.Local.Application.Timesheets.Validators;
using Azure.Local.Application.Timesheets.Workflows;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class TimesheetItemAndResultUnitTests
    {
        [Fact]
        public void TimesheetItem_TotalUnits_ShouldSumAllComponentUnits()
        {
            var item = CreateTimesheet(TimesheetStatus.Draft);
            item.Components =
            [
                CreateComponent(8, item.From, item.From.AddHours(8)),
                CreateComponent(3.5, item.From.AddHours(9), item.From.AddHours(12.5))
            ];

            item.TotalUnits.Should().Be(11.5);
        }

        [Theory]
        [InlineData(TimesheetStatus.Draft, true)]
        [InlineData(TimesheetStatus.Recalled, true)]
        [InlineData(TimesheetStatus.Submitted, false)]
        [InlineData(TimesheetStatus.Approved, false)]
        [InlineData(TimesheetStatus.Rejected, false)]
        public void TimesheetItem_CanEdit_ShouldMatchStatus(TimesheetStatus status, bool expected)
        {
            var item = CreateTimesheet(status);

            item.CanEdit().Should().Be(expected);
        }

        [Fact]
        public void TimesheetItem_CanSubmit_ShouldRequireDraftAndAtLeastOneComponent()
        {
            var draftNoComponents = CreateTimesheet(TimesheetStatus.Draft);
            draftNoComponents.Components = [];

            var draftWithComponent = CreateTimesheet(TimesheetStatus.Draft);
            draftWithComponent.Components = [CreateComponent(8, draftWithComponent.From, draftWithComponent.From.AddHours(8))];

            var submittedWithComponent = CreateTimesheet(TimesheetStatus.Submitted);
            submittedWithComponent.Components = [CreateComponent(8, submittedWithComponent.From, submittedWithComponent.From.AddHours(8))];

            draftNoComponents.CanSubmit().Should().BeFalse();
            draftWithComponent.CanSubmit().Should().BeTrue();
            submittedWithComponent.CanSubmit().Should().BeFalse();
        }

        [Theory]
        [InlineData(TimesheetStatus.Submitted, true)]
        [InlineData(TimesheetStatus.Draft, false)]
        [InlineData(TimesheetStatus.Approved, false)]
        public void TimesheetItem_CanApproveRejectRecall_ShouldBeTrueOnlyWhenSubmitted(TimesheetStatus status, bool expected)
        {
            var item = CreateTimesheet(status);

            item.CanApprove().Should().Be(expected);
            item.CanReject().Should().Be(expected);
            item.CanRecall().Should().Be(expected);
        }

        [Fact]
        public void ValidationResult_FactoryMethods_ShouldCreateExpectedObjects()
        {
            var success = ValidationResult.Success();
            var failureSingle = ValidationResult.Failure("error");
            var failureList = ValidationResult.Failure(["e1", "e2"]);

            success.IsValid.Should().BeTrue();
            success.Errors.Should().BeEmpty();
            failureSingle.IsValid.Should().BeFalse();
            failureSingle.Errors.Should().ContainSingle().Which.Should().Be("error");
            failureList.IsValid.Should().BeFalse();
            failureList.Errors.Should().BeEquivalentTo(["e1", "e2"]);
        }

        [Fact]
        public void TimesheetWorkflowResult_FactoryMethods_ShouldCreateExpectedObjects()
        {
            var success = TimesheetWorkflowResult.Success("ok");
            var failureSingle = TimesheetWorkflowResult.Failure("error");
            var failureList = TimesheetWorkflowResult.Failure(["e1", "e2"]);

            success.IsSuccess.Should().BeTrue();
            success.Message.Should().Be("ok");
            success.Errors.Should().BeEmpty();
            failureSingle.IsSuccess.Should().BeFalse();
            failureSingle.Errors.Should().ContainSingle().Which.Should().Be("error");
            failureList.IsSuccess.Should().BeFalse();
            failureList.Errors.Should().BeEquivalentTo(["e1", "e2"]);
        }

        private static TimesheetItem CreateTimesheet(TimesheetStatus status)
            => new()
            {
                PersonId = "person-1",
                CreatedBy = "person-1",
                Status = status,
                From = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                To = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            };

        private static TimesheetComponentItem CreateComponent(double units, DateTime from, DateTime to)
            => new()
            {
                Units = units,
                From = from,
                To = to,
                TimeCode = "DEV",
                ProjectCode = "PRJ"
            };
    }
}
