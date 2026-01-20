using Azure.Local.Infrastructure.Timesheets;

namespace Azure.Local.Infrastructure.Repository.Specifications
{
    public class DeleteByIdSpecification : GenericSpecification<TimesheetRepositoryItem>
    {
        public DeleteByIdSpecification(string Id)
            => Expression = obj => obj.Id == Id;
    }
}
