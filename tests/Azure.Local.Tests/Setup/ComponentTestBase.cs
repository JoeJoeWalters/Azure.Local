using Azure.Local.Domain;
using Azure.Local.Infrastructure.Repository;
using Microsoft.AspNetCore.TestHost;

namespace Azure.Local.ApiService.Tests.Component.Setup
{
    public abstract class ComponentTestBase : IClassFixture<ApiServiceWebApplicationFactory>, IDisposable
    {
        protected readonly ApiServiceWebApplicationFactory _factory;
        protected HttpClient _client;

        protected ComponentTestBase(ApiServiceWebApplicationFactory factory)
        {
            _factory = factory;
            //_factory.WithWebHostBuilder(builder =>
            //{
            //});
            _client = _factory.CreateDefaultClient();
            _client.DefaultRequestHeaders.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
