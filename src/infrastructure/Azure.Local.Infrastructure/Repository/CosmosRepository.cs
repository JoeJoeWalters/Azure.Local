using Azure.Local.Infrastructure.Repository.Specifications;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Net;

// Notes:
// https://www.aaron-powell.com/posts/2022-08-24-improved-local-dev-with-cosmosdb-and-devcontainers/
// https://github.com/Azure/azure-cosmos-db-emulator-docker/issues/117

namespace Azure.Local.Infrastructure.Repository
{
    [ExcludeFromCodeCoverage]
    public class CosmosRepository<T> : IRepository<T> where T : RepositoryItem
    {
        private readonly CosmosClient _client;
        private readonly Lazy<Task<Container>> _containerLazy;

        public CosmosRepository(IOptions<CosmosRepositorySettings> connectionOptions)
        {
            var settings = connectionOptions.Value;

            _client = new CosmosClient(settings.ConnectionString, clientOptions: CreateClientOptions());
            _containerLazy = new Lazy<Task<Container>>(() => InitialiseAsync(settings.DatabaseId, settings.ContainerId));
        }

        private async Task<Container> InitialiseAsync(string databaseId, string containerId)
        {
            var dbResult = await _client.CreateDatabaseIfNotExistsAsync(databaseId);
            var containerResult = await dbResult.Database.CreateContainerIfNotExistsAsync(containerId, "/id");
            return containerResult.Container;
        }

        private Task<Container> GetContainerAsync() => _containerLazy.Value;

        private static CosmosClientOptions CreateClientOptions() => new()
        {
            HttpClientFactory = () => new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (req, cert, chain, errors) => true
            }),
            ConnectionMode = ConnectionMode.Gateway,
            LimitToEndpoint = true
        };

        public async Task<bool> AddAsync(T item)
        {
            var container = await GetContainerAsync();
            var result = await container.CreateItemAsync(item, new PartitionKey(item.Id));
            return result.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> UpdateAsync(T item)
        {
            var container = await GetContainerAsync();
            var result = await container.ReplaceItemAsync(item, item.Id, new PartitionKey(item.Id));
            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> UpsertAsync(T item)
        {
            var container = await GetContainerAsync();
            var result = await container.UpsertItemAsync(item, new PartitionKey(item.Id));
            return result.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created;
        }

        public async Task<IEnumerable<T>> QueryAsync(GenericSpecification<T> specification, int take = 0)
        {
            var container = await GetContainerAsync();
            IQueryable<T> query = container.GetItemLinqQueryable<T>().Where(specification.Expression);
            if (take > 0)
                query = query.Take(take);

            using var iterator = query.ToFeedIterator();
            var results = new List<T>();
            while (iterator.HasMoreResults)
            {
                var page = await iterator.ReadNextAsync();
                results.AddRange(page);
            }
            return results;
        }

        public async Task<bool> DeleteAsync(GenericSpecification<T> expression)
        {
            var container = await GetContainerAsync();
            var items = await QueryAsync(expression);
            if (!items.Any())
                return false;

            bool success = true;
            foreach (var item in items)
            {
                var response = await container.DeleteItemAsync<T>(item.Id, new PartitionKey(item.Id));
                if (response.StatusCode != HttpStatusCode.NoContent)
                    success = false;
            }
            return success;
        }
    }
}
