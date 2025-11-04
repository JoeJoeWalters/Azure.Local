using Azure.Local.Infrastructure.Repository.Specifications;

namespace Azure.Local.Infrastructure.Test.Specifications
{
    public class TestItemGetSpecification : GenericSpecification<RepositoryTestItem>
    {
        public TestItemGetSpecification(string Id)
            => Expression = obj => obj.Id == Id; 
    }
}
