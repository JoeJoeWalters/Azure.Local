using Azure.Local.Infrastructure.Repository;
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
    }
}
