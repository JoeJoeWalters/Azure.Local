namespace Azure.Local.Infrastructure.Repository
{
    public class CosmosRepositorySettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseId { get; set; } = string.Empty;
        public string ContainerId { get; set; } = string.Empty;
    }
}
