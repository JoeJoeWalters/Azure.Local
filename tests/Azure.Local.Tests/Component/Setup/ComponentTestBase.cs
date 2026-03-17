using Azure.Local.ApiService;
using Azure.Local.ApiService.Versioning;
using LightBDD.XUnit2;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Runtime.CompilerServices;

[assembly: LightBddScope]
namespace Azure.Local.Tests.Component.Setup
{
    [ExcludeFromCodeCoverage]
    public abstract class ComponentTestBase<T> : FeatureFixture, IClassFixture<T>, IDisposable where T : WebApplicationFactory<Program>
    {
        protected readonly T _factory;
        protected HttpClient _client;

        protected ComponentTestBase(T factory)
        {
            _factory = factory;
            _client = _factory.CreateDefaultClient();
            _client.DefaultRequestHeaders.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add(ApiVersioningConstants.HeaderName, ApiVersioningConstants.V1);
        }

        public HttpClient ClonedHttpClient()
        {
            var client = _factory.CreateDefaultClient();
            client.DefaultRequestHeaders.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add(ApiVersioningConstants.HeaderName, ApiVersioningConstants.V1);
            return client;
        }

        ~ComponentTestBase()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
