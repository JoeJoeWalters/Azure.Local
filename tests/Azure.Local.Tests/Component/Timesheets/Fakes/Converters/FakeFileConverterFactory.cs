using Azure.Local.Infrastructure.Timesheets.FileProcessing;
using Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters;

namespace Azure.Local.Tests.Component.Timesheets.Fakes.Converters
{
    public class FakeFileConverterFactory : IFileConverterFactory
    {
        public IFileConverter CreateConverter(TimesheetFileTypes fileType)
        {
            return fileType switch
            {
                TimesheetFileTypes.StandardCSVTemplate => new FakeFileConverter(),
                TimesheetFileTypes.None => throw new ArgumentException("File type cannot be None", nameof(fileType)),
                _ => throw new NotSupportedException($"File type '{fileType}' is not supported")
            };
        }
    }
}
