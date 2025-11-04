using Azure.Local.Infrastructure.Repository;

namespace Azure.Local.Infrastructure.Test
{
    public class RepositoryTestItem : IRepositoryItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
    }
}
