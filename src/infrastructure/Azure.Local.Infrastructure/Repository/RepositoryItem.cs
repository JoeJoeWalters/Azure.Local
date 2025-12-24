using Newtonsoft.Json;

namespace Azure.Local.Infrastructure.Repository
{
    public abstract class RepositoryItem
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
