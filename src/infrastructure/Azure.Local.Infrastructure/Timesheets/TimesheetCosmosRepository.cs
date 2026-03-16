using Azure.Local.Application.Timesheets;
using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Repository.Specifications;
using Azure.Local.Infrastructure.Repository.Specifications.Timesheets;

namespace Azure.Local.Infrastructure.Timesheets
{
    public class TimesheetCosmosRepository(IRepository<TimesheetRepositoryItem> repository) : ITimesheetRepository
    {
        public Task<bool> AddAsync(TimesheetItem item)
            => repository.AddAsync(item.ToTimesheetRepositoryItem());

        public Task<bool> UpdateAsync(TimesheetItem item)
            => repository.UpdateAsync(item.ToTimesheetRepositoryItem());

        public async Task<TimesheetItem?> GetByIdAsync(string id)
        {
            var results = await repository.QueryAsync(new GetByIdSpecification(id), 1);
            return results.FirstOrDefault()?.ToTimesheetItem();
        }

        public Task<bool> DeleteByIdAsync(string id)
            => repository.DeleteAsync(new DeleteByIdSpecification(id));

        public async Task<IEnumerable<TimesheetItem>> SearchAsync(string personId, DateTime fromDate, DateTime toDate)
        {
            var results = await repository.QueryAsync(new TimesheetSearchSpecification(personId, fromDate, toDate));
            return results.Select(item => item.ToTimesheetItem());
        }
    }
}
