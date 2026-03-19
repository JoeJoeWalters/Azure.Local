using Azure.Local.Application.Timesheets;
using Azure.Local.Application.Timesheets.FileProcessing;
using Azure.Local.Application.Timesheets.Workflows;
using Azure.Local.Domain.Timesheets;
using Azure.Local.Tests.Component.Timesheets.Fakes.Repositories;

namespace Azure.Local.Tests.Unit.Timesheets.State
{
    [ExcludeFromCodeCoverage]
    public class TimesheetApplicationStateUnitTests
    {
        [Theory]
        [InlineData(TimesheetStateAction.Submit, "submitted")]
        [InlineData(TimesheetStateAction.Approve, "approved")]
        [InlineData(TimesheetStateAction.Reject, "rejected")]
        [InlineData(TimesheetStateAction.Recall, "recalled")]
        public async Task ChangeStateAsync_ShouldRouteToExpectedWorkflowAction(TimesheetStateAction action, string expectedMessage)
        {
            var repository = new FakeTimesheetRepository();
            var workflow = new SpyWorkflow();
            var app = new TimesheetApplication(repository, new NullFileProcessor(), workflow);
            var timesheet = CreateDraftTimesheet("person-1", "ts-1");
            _ = await repository.AddAsync(timesheet);

            var message = await app.ChangeStateAsync("person-1", timesheet.Id, "actor-1", action, "reason");

            message.Should().Be(expectedMessage);
        }

        [Fact]
        public async Task ChangeStateAsync_ShouldReturnNull_WhenTimesheetNotFound()
        {
            var app = new TimesheetApplication(new FakeTimesheetRepository(), new NullFileProcessor(), new SpyWorkflow());

            var message = await app.ChangeStateAsync("person-1", "missing", "actor-1", TimesheetStateAction.Submit, null);

            message.Should().BeNull();
        }

        [Fact]
        public async Task ChangeStateAsync_ShouldThrow_WhenWorkflowFails()
        {
            var repository = new FakeTimesheetRepository();
            var app = new TimesheetApplication(repository, new NullFileProcessor(), new FailingWorkflow());
            var timesheet = CreateDraftTimesheet("person-1", "ts-1");
            _ = await repository.AddAsync(timesheet);

            var act = async () => await app.ChangeStateAsync("person-1", timesheet.Id, "actor-1", TimesheetStateAction.Submit, null);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*failure*");
        }

        [Fact]
        public async Task ChangeStateAsync_ShouldThrow_ForInvalidStateAction()
        {
            var repository = new FakeTimesheetRepository();
            var app = new TimesheetApplication(repository, new NullFileProcessor(), new SpyWorkflow());
            var timesheet = CreateDraftTimesheet("person-1", "ts-1");
            _ = await repository.AddAsync(timesheet);

            var act = async () => await app.ChangeStateAsync("person-1", timesheet.Id, "actor-1", (TimesheetStateAction)999, null);

            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task SubmitAsync_ShouldReturnTrue_WhenFound()
        {
            var repository = new FakeTimesheetRepository();
            var app = new TimesheetApplication(repository, new NullFileProcessor(), new SpyWorkflow());
            var timesheet = CreateDraftTimesheet("person-1", "ts-1");
            _ = await repository.AddAsync(timesheet);

            var result = await app.SubmitAsync("person-1", timesheet.Id, "actor-1");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task ApproveAsync_ShouldReturnTrue_WhenFound()
        {
            var repository = new FakeTimesheetRepository();
            var app = new TimesheetApplication(repository, new NullFileProcessor(), new SpyWorkflow());
            var timesheet = CreateDraftTimesheet("person-1", "ts-1");
            _ = await repository.AddAsync(timesheet);

            var result = await app.ApproveAsync("person-1", timesheet.Id, "actor-1");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task RejectAsync_ShouldReturnTrue_WhenFound()
        {
            var repository = new FakeTimesheetRepository();
            var app = new TimesheetApplication(repository, new NullFileProcessor(), new SpyWorkflow());
            var timesheet = CreateDraftTimesheet("person-1", "ts-1");
            _ = await repository.AddAsync(timesheet);

            var result = await app.RejectAsync("person-1", timesheet.Id, "actor-1", "reason");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task RecallAsync_ShouldReturnTrue_WhenFound()
        {
            var repository = new FakeTimesheetRepository();
            var app = new TimesheetApplication(repository, new NullFileProcessor(), new SpyWorkflow());
            var timesheet = CreateDraftTimesheet("person-1", "ts-1");
            _ = await repository.AddAsync(timesheet);

            var result = await app.RecallAsync("person-1", timesheet.Id, "actor-1");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task ReopenAsync_ShouldReturnTrue_WhenFound()
        {
            var repository = new FakeTimesheetRepository();
            var app = new TimesheetApplication(repository, new NullFileProcessor(), new SpyWorkflow());
            var timesheet = CreateDraftTimesheet("person-1", "ts-1");
            _ = await repository.AddAsync(timesheet);

            var result = await app.ReopenAsync("person-1", timesheet.Id, "actor-1");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task SubmitAsync_ShouldReturnFalse_WhenNotFound()
        {
            var app = new TimesheetApplication(new FakeTimesheetRepository(), new NullFileProcessor(), new SpyWorkflow());

            var result = await app.SubmitAsync("person-1", "missing", "actor-1");

            result.Should().BeFalse();
        }

        private static TimesheetItem CreateDraftTimesheet(string personId, string id)
            => new()
            {
                Id = id,
                PersonId = personId,
                CreatedBy = personId,
                From = DateTime.UtcNow.AddHours(-8),
                To = DateTime.UtcNow,
                Components =
                [
                    new TimesheetComponentItem
                    {
                        Id = $"{id}-c1",
                        Units = 8,
                        From = DateTime.UtcNow.AddHours(-8),
                        To = DateTime.UtcNow,
                        TimeCode = "DEV",
                        ProjectCode = "PRJ"
                    }
                ]
            };

        private sealed class NullFileProcessor : ITimesheetFileProcessor
        {
            public Task<TimesheetItem?> ProcessFileAsync(string personId, Stream fileStream, TimesheetFileTypes fileType)
                => Task.FromResult<TimesheetItem?>(null);
        }

        private sealed class SpyWorkflow : ITimesheetWorkflow
        {
            public TimesheetWorkflowResult Submit(TimesheetItem timesheet, string submittedBy)
                => TimesheetWorkflowResult.Success("submitted");

            public TimesheetWorkflowResult Approve(TimesheetItem timesheet, string approvedBy)
                => TimesheetWorkflowResult.Success("approved");

            public TimesheetWorkflowResult Reject(TimesheetItem timesheet, string rejectedBy, string reason)
                => TimesheetWorkflowResult.Success("rejected");

            public TimesheetWorkflowResult Recall(TimesheetItem timesheet, string recalledBy)
                => TimesheetWorkflowResult.Success("recalled");

            public TimesheetWorkflowResult Reopen(TimesheetItem timesheet, string reopenedBy)
                => TimesheetWorkflowResult.Success("reopened");
        }

        private sealed class FailingWorkflow : ITimesheetWorkflow
        {
            private static TimesheetWorkflowResult Fail() => TimesheetWorkflowResult.Failure("workflow failure");

            public TimesheetWorkflowResult Submit(TimesheetItem timesheet, string submittedBy) => Fail();
            public TimesheetWorkflowResult Approve(TimesheetItem timesheet, string approvedBy) => Fail();
            public TimesheetWorkflowResult Reject(TimesheetItem timesheet, string rejectedBy, string reason) => Fail();
            public TimesheetWorkflowResult Recall(TimesheetItem timesheet, string recalledBy) => Fail();
            public TimesheetWorkflowResult Reopen(TimesheetItem timesheet, string reopenedBy) => Fail();
        }
    }
}
