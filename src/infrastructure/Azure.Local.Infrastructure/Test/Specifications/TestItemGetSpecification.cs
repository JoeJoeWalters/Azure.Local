using Azure.Local.Infrastructure.Repository.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Local.Infrastructure.Test.Specifications
{
    public class TestItemGetSpecification : GenericSpecification<RepositoryTestItem>
    {
        public TestItemGetSpecification(string Id)
            => Expression = obj => obj.Id == Id; 
    }
}
