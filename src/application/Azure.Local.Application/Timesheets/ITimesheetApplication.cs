using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Application.Timesheets
{
    public interface ITimesheetApplication
    {
        Task<bool> SaveAsync(TimesheetItem item);
        Task<TimesheetItem?> GetAsync(string id);
    }
}
