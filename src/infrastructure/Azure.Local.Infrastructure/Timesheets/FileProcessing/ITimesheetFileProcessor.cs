using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Infrastructure.Timesheets.FileProcessing
{
    public interface ITimesheetFileProcessor
    {
        Task<TimesheetItem?> ProcessFileAsync(string personId, Stream fileStream, TimesheetFileTypes fileType);
    }
}
