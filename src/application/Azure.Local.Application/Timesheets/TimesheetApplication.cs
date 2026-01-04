using Azure.Local.Application.Timesheets.Helpers;
using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Repository.Specifications;
using Azure.Local.Infrastructure.Repository.Specifications.Timesheets;
using Azure.Local.Infrastructure.Timesheets;
using Azure.Local.Infrastructure.Timesheets.FileProcessing;

namespace Azure.Local.Application.Timesheets
{
    public class TimesheetApplication(IRepository<TimesheetRepositoryItem> repository, ITimesheetFileProcessor fileProcessor) : ITimesheetApplication
    {
        private readonly IRepository<TimesheetRepositoryItem> _repository = repository;

        public Task<bool> AddAsync(string personId, TimesheetItem item)
            => _repository.AddAsync(item.ToTimesheetRepositoryItem());

        public Task<bool> UpdateAsync(string personId, TimesheetItem item)
            => _repository.UpdateAsync(item.ToTimesheetRepositoryItem());

        public Task<TimesheetItem?> GetAsync(string personId, string id)
        {
            var queryResult = _repository.QueryAsync(new GetByIdSpecification(id), 1);
            if (queryResult.Result.Any())
                return Task.FromResult((TimesheetItem?)queryResult.Result.First().ToTimesheetItem());
            else
                return Task.FromResult((TimesheetItem?)null);
        }

        public Task<bool> DeleteAsync(string personId, string id)
            => _repository.DeleteAsync(new DeleteByIdSpecification(id));        

        public Task<List<TimesheetItem>> SearchAsync(string personId, DateTime fromDate, DateTime toDate)
        {
            var queryResult = _repository.QueryAsync(new TimesheetSearchSpecification(personId, fromDate, toDate));
            return Task.FromResult(queryResult.Result.Select(item => item.ToTimesheetItem()).ToList());
        }

        public Task<TimesheetItem?> ProcessFileAsync(string personId, Stream stream, TimesheetFileTypes fileType)
            => fileProcessor.ProcessFileAsync(personId, stream, fileType);
    }
}
