using Azure.Local.Application.Timesheets.Helpers;
using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Repository.Specifications;
using Azure.Local.Infrastructure.Timesheets;

namespace Azure.Local.Application.Timesheets
{
    public class TimesheetApplication : ITimesheetApplication
    {
        private readonly IRepository<TimesheetRepositoryItem> _repository;

        public TimesheetApplication(IRepository<TimesheetRepositoryItem> repository)
        {
            _repository = repository;
        }

        public Task<bool> AddAsync(TimesheetItem item)
        {
            return _repository.AddAsync(item.ToTimesheetRepositoryItem());
        }

        public Task<bool> UpdateAsync(TimesheetItem item)
        {
            return _repository.UpdateAsync(item.ToTimesheetRepositoryItem());
        }

        public Task<TimesheetItem?> GetAsync(string id)
        {
            var queryResult = _repository.QueryAsync(new GetByIdSpecification(id), 1);
            if (queryResult.Result.Any())
                return Task.FromResult(queryResult.Result?.First().ToTimesheetItem());
            else
                return Task.FromResult((TimesheetItem?)null);
        }

        public Task<bool> DeleteAsync(string id)
        {
            return _repository.DeleteAsync(new DeleteByIdSpecification(id));
        }
    }
}
