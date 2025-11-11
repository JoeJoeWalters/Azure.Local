using Azure.Local.Infrastructure.Repository.Specifications;
using Microsoft.Azure.Cosmos;
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
        private readonly Container _container;

        public CosmosRepository(IOptions<CosmosRepositorySettings> connectionOptions)
        {
            // Initialize Cosmos DB client and container but protect it from taking the app down
            // if it is running component tests without Cosmos DB available.
            try
            {
                // For local emulator or self-signed certificates, bypass certificate validation
                // https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-develop-emulator?tabs=windows%2Ccsharp&pivots=api-nosql
                CosmosClientOptions options =
                    new CosmosClientOptions
                    {
                        HttpClientFactory = () =>
                        {
                            HttpMessageHandler httpMessageHandler = new HttpClientHandler()
                            {
                                ServerCertificateCustomValidationCallback = (req, cert, chain, errors) => true
                            };

                            return new HttpClient(httpMessageHandler);
                        },
                        ConnectionMode = ConnectionMode.Gateway,
                        LimitToEndpoint = true
                    };

                _client = new CosmosClient(connectionOptions.Value.ConnectionString, clientOptions: options);
                var dbResult = _client.CreateDatabaseIfNotExistsAsync(connectionOptions.Value.DatabaseId).GetAwaiter().GetResult();
                if (dbResult.Database != null)
                {
                    var containerResult = dbResult.Database.CreateContainerIfNotExistsAsync(connectionOptions.Value.ContainerId, "/id").GetAwaiter().GetResult();
                    if (containerResult.Container != null)
                    {
                        _container = containerResult.Container;
                    }
                }
            }
            catch (CosmosException ex)
            {
                // Handle Cosmos DB specific exceptions
                throw new InvalidOperationException("Failed to initialize Cosmos DB client or container.", ex);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                throw new InvalidOperationException("An error occurred while initializing the repository.", ex);
            }
        }

        public async Task<bool> AddAsync(T item)
        {
            var result = await _container.CreateItemAsync(item, new PartitionKey(item.Id));
            return (result.StatusCode == HttpStatusCode.OK);
        }

        public async Task<bool> UpdateAsync(T item)
        {
            var result = await _container.ReplaceItemAsync(item, item.Id, new PartitionKey(item.Id));
            return (result.StatusCode == HttpStatusCode.OK);
        }

        public async Task<bool> UpsertAsync(T item)
        {
            var result = await _container.UpsertItemAsync(item, new PartitionKey(item.Id));
            return (result.StatusCode == HttpStatusCode.OK);
        }

        public async Task<IEnumerable<T>> QueryAsync(GenericSpecification<T> specification, int take = 0)
        {
            var queryable = _container.GetItemLinqQueryable<T>(allowSynchronousQueryExecution: true)
                .Where(specification.Expression.Compile());

            if (take > 0)
                queryable = queryable.Take(take);

            return queryable.ToList();
        }
    }
}
