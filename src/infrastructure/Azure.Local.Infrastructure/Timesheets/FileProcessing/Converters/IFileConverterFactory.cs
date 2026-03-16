using Azure.Local.Application.Timesheets.FileProcessing;

namespace Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters
{
    public interface IFileConverterFactory
    {
        IFileConverter CreateConverter(TimesheetFileTypes fileType);
    }
}
