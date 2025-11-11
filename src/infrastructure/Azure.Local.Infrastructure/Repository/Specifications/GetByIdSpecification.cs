using Azure.Local.Infrastructure.Timesheets;

namespace Azure.Local.Infrastructure.Repository.Specifications
{
    public class GetByIdSpecification : GenericSpecification<TimesheetRepositoryItem>
    {
        public GetByIdSpecification(string Id)
            => Expression = obj => obj.Id == Id; 
    }
}
