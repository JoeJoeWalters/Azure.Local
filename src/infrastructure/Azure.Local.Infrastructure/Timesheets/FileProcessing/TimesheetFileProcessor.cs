using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters;

namespace Azure.Local.Infrastructure.Timesheets.FileProcessing
{
    public class TimesheetFileProcessor(
        IRepository<TimesheetRepositoryItem> repository,
        IFileConverterFactory converterFactory) : ITimesheetFileProcessor
    {
        public async Task<TimesheetItem?> ProcessFileAsync(string personId, Stream fileStream, TimesheetFileTypes fileType)
        {
            ArgumentNullException.ThrowIfNull(fileStream);

            var converter = converterFactory.CreateConverter(fileType);
            var timesheetItem = await converter.ConvertAsync(personId, fileStream);

            if (timesheetItem != null)
            {
                var repositoryItem = new TimesheetRepositoryItem
                {
                    From = timesheetItem.From,
                    PersonId = timesheetItem.PersonId,
                    To = timesheetItem.To,
                    CreatedBy = timesheetItem.CreatedBy,
                    CreatedDate = timesheetItem.CreatedDate,
                    ModifiedDate = timesheetItem.ModifiedDate,
                    Status = timesheetItem.Status,
                    ManagerId = timesheetItem.ManagerId,
                    Comments = timesheetItem.Comments,
                    Components = [.. timesheetItem.Components.Select(c => new TimesheetComponentRepositoryItem
                    {
                        Id = c.Id,
                        From = c.From,
                        To = c.To,
                        Units = c.Units,
                        TimeCode = c.TimeCode,
                        ProjectCode = c.ProjectCode,
                        Description = c.Description,
                        WorkType = c.WorkType,
                        IsBillable = c.IsBillable,
                        IsLocked = c.IsLocked
                    })]
                };

                await repository.AddAsync(repositoryItem);
            }

            return timesheetItem;
        }
    }
}
