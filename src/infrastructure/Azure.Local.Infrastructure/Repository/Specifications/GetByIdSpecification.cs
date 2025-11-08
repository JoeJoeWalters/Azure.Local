using Azure.Local.Infrastructure.Repository.Specifications;
using Azure.Local.Infrastructure.Timesheets;

namespace Azure.Local.Infrastructure.Test.Specifications
{
    public class GetByIdSpecification : GenericSpecification<TimesheetRepositoryItem>
    {
        public GetByIdSpecification(string Id)
            => Expression = obj => obj.Id == Id; 
    }
}
