using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository;

namespace Azure.Local.Infrastructure.Timesheets.FileProcessing
{
    public class TimesheetFileProcessor(IRepository<TimesheetRepositoryItem> repository) : ITimesheetFileProcessor
    {
        public Task<TimesheetItem?> ProcessFileAsync(Stream stream)
        {
            repository.AddAsync(new TimesheetRepositoryItem() { From = DateTime.UtcNow, PersonId = Guid.NewGuid().ToString(), To = DateTime.UtcNow });

            throw new NotImplementedException();
        }
    }
}
