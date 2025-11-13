using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Application.Timesheets
{
    public interface ITimesheetApplication
    {
        Task<bool> AddAsync(TimesheetItem item);
        Task<bool> UpdateAsync(TimesheetItem item);
        Task<TimesheetItem?> GetAsync(string id);
        Task<bool> DeleteAsync(string id);
    }
}
