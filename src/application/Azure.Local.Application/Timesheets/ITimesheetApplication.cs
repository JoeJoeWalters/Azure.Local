using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Application.Timesheets
{
    public interface ITimesheetApplication
    {
        bool Save(TimesheetItem item);
        TimesheetItem? GetById(string id);
    }
}
