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
    }
}
