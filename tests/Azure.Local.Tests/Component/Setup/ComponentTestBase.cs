using Microsoft.AspNetCore.Mvc.Testing;

namespace Azure.Local.ApiService.Tests.Component.Setup
{
    public abstract class ComponentTestBase<T> : IClassFixture<T>, IDisposable where T: WebApplicationFactory<Program>
    {
        protected readonly T _factory;
        protected HttpClient _client;

        protected ComponentTestBase(T factory)
        {
            _factory = factory;
            //_factory.WithWebHostBuilder(builder =>
            //{
            //});
            _client = _factory.CreateDefaultClient();
            _client.DefaultRequestHeaders.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        ~ComponentTestBase()
        {
            Dispose();
        }

        public void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
