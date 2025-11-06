using Azure.Local.Infrastructure.Repository;

namespace Azure.Local.Infrastructure.Test
{
    public class RepositoryTestItem : RepositoryItem
    {
        public RepositoryTestItem() { 
            Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; } = string.Empty;
    }
}
