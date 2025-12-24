using Azure.Local.Infrastructure.Timesheets;

namespace Azure.Local.Infrastructure.Repository.Specifications.Timesheets
{
    public class TimesheetSearchSpecification : GenericSpecification<TimesheetRepositoryItem>
    {
        public TimesheetSearchSpecification(string personId, DateTime? fromDate, DateTime? toDate)
            => Expression = obj =>
                (obj.PersonId == personId && (obj.From <= toDate && obj.To >= fromDate));
    }
}
