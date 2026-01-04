using Microsoft.Extensions.DependencyInjection;

namespace Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters
{
    public class FileConverterFactory(IServiceProvider serviceProvider) : IFileConverterFactory
    {
        public IFileConverter CreateConverter(TimesheetFileTypes fileType)
        {
            return fileType switch
            {
                TimesheetFileTypes.StandardCSVTemplate => serviceProvider.GetRequiredService<StandardCsvFileConverter>(),
                TimesheetFileTypes.None => throw new ArgumentException("File type cannot be None", nameof(fileType)),
                _ => throw new NotSupportedException($"File type '{fileType}' is not supported")
            };
        }
    }
}
