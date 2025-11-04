using Azure.Local.Infrastructure.Repository.Specifications;
using Microsoft.Azure.Cosmos;
using System.Linq.Expressions;

namespace Azure.Local.Infrastructure.Repository
{
    public class CosmosRepository<T> : IRepository<T> where T : IRepositoryItem
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public CosmosRepository(string connectionString, string databaseId, string containerId)
        {
            _client = new CosmosClient(connectionString);
            _container = _client.GetContainer(databaseId, containerId);
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
