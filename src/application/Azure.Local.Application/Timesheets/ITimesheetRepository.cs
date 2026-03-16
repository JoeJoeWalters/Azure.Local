using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Application.Timesheets
{
    public interface ITimesheetRepository
    {
        Task<bool> AddAsync(TimesheetItem item);
        Task<bool> UpdateAsync(TimesheetItem item);
        Task<TimesheetItem?> GetByIdAsync(string id);
        Task<bool> DeleteByIdAsync(string id);
        Task<IEnumerable<TimesheetItem>> SearchAsync(string personId, DateTime fromDate, DateTime toDate);
    }
}
