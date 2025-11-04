using Azure.Local.Infrastructure.Repository.Specifications;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Azure.Local.Infrastructure.Repository
{
    public class CosmosRepository<T> : IRepository<T> where T : IRepositoryItem
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public CosmosRepository(IOptions<CosmosRepositorySettings> connectionOptions)
        {
            // Initialize Cosmos DB client and container but protect it from taking the app down
            // if it is running component tests without Cosmos DB available.
            try
            {
                _client = new CosmosClient(connectionOptions.Value.ConnectionString);
                _container = _client.GetContainer(connectionOptions.Value.DatabaseId, connectionOptions.Value.ContainerId);
            }
            catch (CosmosException ex)
            {
                // Handle Cosmos DB specific exceptions
                //throw new InvalidOperationException("Failed to initialize Cosmos DB client or container.", ex);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                //throw new InvalidOperationException("An error occurred while initializing the repository.", ex);
            }
        }

        public async void Add(T item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        public async void Update(T item)
        {
            await _container.ReplaceItemAsync(item, item.Id, new PartitionKey(item.Id));
        }

        public async void Upsert(T item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(item.Id));
        }

        public async Task<IEnumerable<T>> Query(GenericSpecification<T> specification, int take = 0)
        {
            var queryable = _container.GetItemLinqQueryable<T>(allowSynchronousQueryExecution: true)
                .Where(specification.Expression.Compile());

            if (take > 0)
                queryable = queryable.Take(take);

            return queryable.ToList();
        }
    }
}
