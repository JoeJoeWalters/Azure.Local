using Azure.Local.Application.Timesheets;
using Azure.Local.Application.Timesheets.FileProcessing;
using Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters;

namespace Azure.Local.Infrastructure.Timesheets.FileProcessing
{
    public class TimesheetFileProcessor(
        ITimesheetRepository repository,
        IFileConverterFactory converterFactory) : ITimesheetFileProcessor
    {
        public async Task<Domain.Timesheets.TimesheetItem?> ProcessFileAsync(string personId, System.IO.Stream fileStream, TimesheetFileTypes fileType)
        {
            ArgumentNullException.ThrowIfNull(fileStream);

            var converter = converterFactory.CreateConverter(fileType);
            var timesheetItem = await converter.ConvertAsync(personId, fileStream);

            if (timesheetItem != null)
                await repository.AddAsync(timesheetItem);

            return timesheetItem;
        }
    }
}
