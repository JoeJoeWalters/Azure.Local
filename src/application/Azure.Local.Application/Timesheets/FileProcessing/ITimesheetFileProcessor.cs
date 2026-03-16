using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Application.Timesheets.FileProcessing
{
    public interface ITimesheetFileProcessor
    {
        Task<TimesheetItem?> ProcessFileAsync(string personId, Stream fileStream, TimesheetFileTypes fileType);
    }
}
