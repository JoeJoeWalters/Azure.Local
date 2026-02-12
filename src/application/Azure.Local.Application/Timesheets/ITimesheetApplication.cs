using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Timesheets.FileProcessing;

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

        // Workflow methods
        Task<bool> ChangeStateAsync(string personId, string timesheetId, string actionBy, TimesheetStateAction state, string? comments);
        Task<bool> SubmitAsync(string personId, string timesheetId, string submittedBy);
        Task<bool> ApproveAsync(string personId, string timesheetId, string approvedBy);
        Task<bool> RejectAsync(string personId, string timesheetId, string rejectedBy, string reason);
        Task<bool> RecallAsync(string personId, string timesheetId, string recalledBy);
        Task<bool> ReopenAsync(string personId, string timesheetId, string reopenedBy);
    }
}
