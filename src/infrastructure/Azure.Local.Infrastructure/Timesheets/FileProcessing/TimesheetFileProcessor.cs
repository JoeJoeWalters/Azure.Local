using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters;

namespace Azure.Local.Infrastructure.Timesheets.FileProcessing
{
    public class TimesheetFileProcessor(
        IRepository<TimesheetRepositoryItem> repository,
        IFileConverterFactory converterFactory) : ITimesheetFileProcessor
    {
        public async Task<TimesheetItem?> ProcessFileAsync(Stream fileStream, TimesheetFileTypes fileType)
        {
            ArgumentNullException.ThrowIfNull(fileStream);

            var converter = converterFactory.CreateConverter(fileType);
            var timesheetItem = await converter.ConvertAsync(fileStream);

            if (timesheetItem != null)
            {
                var repositoryItem = new TimesheetRepositoryItem
                {
                    From = timesheetItem.From,
                    PersonId = timesheetItem.PersonId,
                    To = timesheetItem.To
                };

                await repository.AddAsync(repositoryItem);
            }

            return timesheetItem;
        }
    }
}
