using Azure.Local.Application.Timesheets.Helpers;
using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Test.Specifications;
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

        public bool Save(TimesheetItem item)
        {
            _repository.Add(item.ToTimesheetRepositoryItem());
            return true;
        }

        public TimesheetItem? GetById(string id)
        {
            var queryResult = _repository.Query(new GetByIdSpecification(id), 1);
            if (queryResult.Result.Any())
                return queryResult.Result.First().ToTimesheetItem();
            else
                return null;
        }
    }
}
