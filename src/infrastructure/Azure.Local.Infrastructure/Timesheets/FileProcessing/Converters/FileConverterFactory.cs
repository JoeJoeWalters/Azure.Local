namespace Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters
{
    public class FileConverterFactory() : IFileConverterFactory
    {
        public IFileConverter CreateConverter(TimesheetFileTypes fileType)
        {
            return fileType switch
            {
                TimesheetFileTypes.StandardCSVTemplate => new StandardCsvFileConverter(),
                TimesheetFileTypes.None => throw new ArgumentException("File type cannot be None", nameof(fileType)),
                _ => throw new NotSupportedException($"File type '{fileType}' is not supported")
            };
        }
    }
}
