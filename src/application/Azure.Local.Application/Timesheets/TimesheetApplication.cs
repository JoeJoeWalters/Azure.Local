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
            _repository.Add(new TimesheetRepositoryItem
            {
                Id = item.Id,
                From = item.From,
                To = item.To,
                Components = item.Components.Select(c => new TimesheetComponentRepositoryItem
                {
                    Units = c.Units,
                    From = c.From,
                    To = c.To
                }).ToList()
            });

            return true;
        }

        public TimesheetItem? GetById(string id)
        {
            var queryResult = _repository.Query(new GetByIdSpecification(id), 1);
            if (queryResult.Result.Any())
            {
                var first = queryResult.Result.First();
                var item = new TimesheetItem
                {
                    Id = first.Id,
                    From = first.From,
                    To = first.To,
                    Components = first.Components.Select(c => new TimesheetComponentItem
                    {
                        Units = c.Units,
                        From = c.From,
                        To = c.To
                    }).ToList()
                };

                return item;
            }
            else
                return null;
        }
    }
}
