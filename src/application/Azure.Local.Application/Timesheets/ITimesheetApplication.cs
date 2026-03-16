using Azure.Local.Application.Timesheets.FileProcessing;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Application.Timesheets
{
    public interface ITimesheetApplication
    {
        Task<bool> AddAsync(string personId, TimesheetItem item);
        Task<bool> UpdateAsync(string personId, TimesheetItem item);
        Task<TimesheetItem?> GetAsync(string personId, string id);
        Task<bool> DeleteAsync(string personId, string id);
        Task<List<TimesheetItem>> SearchAsync(string personId, DateTime fromDate, DateTime toDate);
        Task<TimesheetItem?> ProcessFileAsync(string personId, Stream stream, TimesheetFileTypes fileType);

        // Workflow methods — ChangeStateAsync returns the workflow message on success, null if not found
        Task<string?> ChangeStateAsync(string personId, string timesheetId, string actionBy, TimesheetStateAction state, string? comments);
        Task<bool> SubmitAsync(string personId, string timesheetId, string submittedBy);
        Task<bool> ApproveAsync(string personId, string timesheetId, string approvedBy);
        Task<bool> RejectAsync(string personId, string timesheetId, string rejectedBy, string reason);
        Task<bool> RecallAsync(string personId, string timesheetId, string recalledBy);
        Task<bool> ReopenAsync(string personId, string timesheetId, string reopenedBy);
    }
}
