using Azure.Local.Application.Timesheets.FileProcessing;
using Azure.Local.Application.Timesheets.V1;
using Azure.Local.Application.Timesheets.Workflows;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Application.Timesheets
{
    public class TimesheetApplicationV1(
        ITimesheetRepository repository,
        ITimesheetFileProcessor fileProcessor,
        ITimesheetWorkflow workflow) : ITimesheetApplicationV1
    {
        public Task<bool> AddAsync(string personId, TimesheetItem item)
            => repository.AddAsync(item);

        public async Task<bool> UpdateAsync(string personId, TimesheetItem item)
        {
            item.ModifiedDate = DateTime.UtcNow;
            return await repository.UpdateAsync(item);
        }

        public Task<TimesheetItem?> GetAsync(string personId, string id)
            => repository.GetByIdAsync(id);

        public Task<bool> DeleteAsync(string personId, string id)
            => repository.DeleteByIdAsync(id);

        public async Task<List<TimesheetItem>> SearchAsync(string personId, DateTime fromDate, DateTime toDate)
        {
            var results = await repository.SearchAsync(personId, fromDate, toDate);
            return results.ToList();
        }

        public Task<TimesheetItem?> ProcessFileAsync(string personId, Stream stream, TimesheetFileTypes fileType)
            => fileProcessor.ProcessFileAsync(personId, stream, fileType);

        public Task<string?> ChangeStateAsync(string personId, string timesheetId, string actionBy, TimesheetStateAction state, string? comments)
            => state switch
            {
                TimesheetStateAction.Submit => ExecuteWorkflowAsync(personId, timesheetId, ts => workflow.Submit(ts, actionBy)),
                TimesheetStateAction.Approve => ExecuteWorkflowAsync(personId, timesheetId, ts => workflow.Approve(ts, actionBy)),
                TimesheetStateAction.Reject => ExecuteWorkflowAsync(personId, timesheetId, ts => workflow.Reject(ts, actionBy, comments ?? string.Empty)),
                TimesheetStateAction.Recall => ExecuteWorkflowAsync(personId, timesheetId, ts => workflow.Recall(ts, actionBy)),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, "Invalid state action")
            };

        public async Task<bool> SubmitAsync(string personId, string timesheetId, string submittedBy)
            => await ExecuteWorkflowAsync(personId, timesheetId, ts => workflow.Submit(ts, submittedBy)) is not null;

        public async Task<bool> ApproveAsync(string personId, string timesheetId, string approvedBy)
            => await ExecuteWorkflowAsync(personId, timesheetId, ts => workflow.Approve(ts, approvedBy)) is not null;

        public async Task<bool> RejectAsync(string personId, string timesheetId, string rejectedBy, string reason)
            => await ExecuteWorkflowAsync(personId, timesheetId, ts => workflow.Reject(ts, rejectedBy, reason)) is not null;

        public async Task<bool> RecallAsync(string personId, string timesheetId, string recalledBy)
            => await ExecuteWorkflowAsync(personId, timesheetId, ts => workflow.Recall(ts, recalledBy)) is not null;

        public async Task<bool> ReopenAsync(string personId, string timesheetId, string reopenedBy)
            => await ExecuteWorkflowAsync(personId, timesheetId, ts => workflow.Reopen(ts, reopenedBy)) is not null;

        // Returns the workflow message on success, null if the timesheet was not found.
        // Throws InvalidOperationException if a business rule is violated.
        private async Task<string?> ExecuteWorkflowAsync(
            string personId, string timesheetId,
            Func<TimesheetItem, TimesheetWorkflowResult> action)
        {
            var timesheet = await GetAsync(personId, timesheetId);
            if (timesheet is null)
                return null;

            var result = action(timesheet);
            if (!result.IsSuccess)
                throw new InvalidOperationException(string.Join(", ", result.Errors));

            await repository.UpdateAsync(timesheet);
            return result.Message;
        }
    }
}
