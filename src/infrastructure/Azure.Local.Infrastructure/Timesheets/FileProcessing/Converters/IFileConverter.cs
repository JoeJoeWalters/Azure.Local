using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters
{
    public interface IFileConverter
    {
        Task<TimesheetItem?> ConvertAsync(string personId, Stream fileStream);
    }
}
