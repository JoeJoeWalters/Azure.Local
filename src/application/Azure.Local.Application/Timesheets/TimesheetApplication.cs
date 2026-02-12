using Azure.Local.Application.Timesheets.Helpers;
using Azure.Local.Application.Timesheets.Workflows;
using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Repository.Specifications;
using Azure.Local.Infrastructure.Repository.Specifications.Timesheets;
using Azure.Local.Infrastructure.Timesheets;
using Azure.Local.Infrastructure.Timesheets.FileProcessing;

namespace Azure.Local.Application.Timesheets
{
    public class TimesheetApplication(IRepository<TimesheetRepositoryItem> repository, ITimesheetFileProcessor fileProcessor) : ITimesheetApplication
    {
        private readonly IRepository<TimesheetRepositoryItem> _repository = repository;
        private readonly TimesheetWorkflow _workflow = new();

        public Task<bool> AddAsync(string personId, TimesheetItem item)
            => _repository.AddAsync(item.ToTimesheetRepositoryItem());

        public Task<bool> UpdateAsync(string personId, TimesheetItem item)
        {
            item.ModifiedDate = DateTime.UtcNow;
            return _repository.UpdateAsync(item.ToTimesheetRepositoryItem());
        }

        public Task<TimesheetItem?> GetAsync(string personId, string id)
        {
            var queryResult = _repository.QueryAsync(new GetByIdSpecification(id), 1);
            if (queryResult.Result.Any())
                return Task.FromResult((TimesheetItem?)queryResult.Result.First().ToTimesheetItem());
            else
                return Task.FromResult((TimesheetItem?)null);
        }

        public Task<bool> DeleteAsync(string personId, string id)
            => _repository.DeleteAsync(new DeleteByIdSpecification(id));        

        public Task<List<TimesheetItem>> SearchAsync(string personId, DateTime fromDate, DateTime toDate)
        {
            var queryResult = _repository.QueryAsync(new TimesheetSearchSpecification(personId, fromDate, toDate));
            return Task.FromResult(queryResult.Result.Select(item => item.ToTimesheetItem()).ToList());
        }

        public Task<TimesheetItem?> ProcessFileAsync(string personId, Stream stream, TimesheetFileTypes fileType)
            => fileProcessor.ProcessFileAsync(personId, stream, fileType);

        // Workflow methods

        public Task<bool> ChangeStateAsync(string personId, string timesheetId, string actionBy, TimesheetStateAction state, string? comments)
        {
            return state switch
            {
                TimesheetStateAction.Submit => SubmitAsync(personId, timesheetId, actionBy),
                TimesheetStateAction.Approve => ApproveAsync(personId, timesheetId, actionBy),
                TimesheetStateAction.Reject => RejectAsync(personId, timesheetId, actionBy, comments ?? string.Empty),
                TimesheetStateAction.Recall => RecallAsync(personId, timesheetId, actionBy),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, "Invalid state action")
            };
        }

        public async Task<bool> SubmitAsync(string personId, string timesheetId, string submittedBy)
        {
            var timesheet = await GetAsync(personId, timesheetId);
            if (timesheet == null)
                return false;

            var result = _workflow.Submit(timesheet, submittedBy);
            if (!result.IsSuccess)
                throw new InvalidOperationException(string.Join(", ", result.Errors));

            return await UpdateAsync(personId, timesheet);
        }

        public async Task<bool> ApproveAsync(string personId, string timesheetId, string approvedBy)
        {
            var timesheet = await GetAsync(personId, timesheetId);
            if (timesheet == null)
                return false;

            var result = _workflow.Approve(timesheet, approvedBy);
            if (!result.IsSuccess)
                throw new InvalidOperationException(string.Join(", ", result.Errors));

            return await UpdateAsync(personId, timesheet);
        }

        public async Task<bool> RejectAsync(string personId, string timesheetId, string rejectedBy, string reason)
        {
            var timesheet = await GetAsync(personId, timesheetId);
            if (timesheet == null)
                return false;

            var result = _workflow.Reject(timesheet, rejectedBy, reason);
            if (!result.IsSuccess)
                throw new InvalidOperationException(string.Join(", ", result.Errors));

            return await UpdateAsync(personId, timesheet);
        }

        public async Task<bool> RecallAsync(string personId, string timesheetId, string recalledBy)
        {
            var timesheet = await GetAsync(personId, timesheetId);
            if (timesheet == null)
                return false;

            var result = _workflow.Recall(timesheet, recalledBy);
            if (!result.IsSuccess)
                throw new InvalidOperationException(string.Join(", ", result.Errors));

            return await UpdateAsync(personId, timesheet);
        }

        public async Task<bool> ReopenAsync(string personId, string timesheetId, string reopenedBy)
        {
            var timesheet = await GetAsync(personId, timesheetId);
            if (timesheet == null)
                return false;

            var result = _workflow.Reopen(timesheet, reopenedBy);
            if (!result.IsSuccess)
                throw new InvalidOperationException(string.Join(", ", result.Errors));

            return await UpdateAsync(personId, timesheet);
        }
    }
}
