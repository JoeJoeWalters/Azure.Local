using Azure.Local.ApiService;
using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.ApiService.Timesheets.Helpers;
using Azure.Local.Tests.Component.Timesheets;
using LightBDD.XUnit2;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

[assembly: LightBddScope]
namespace Azure.Local.Tests.Component.Setup
{
    public abstract class ComponentTestBase<T> : FeatureFixture, IClassFixture<T>, IDisposable where T: WebApplicationFactory<Program>
    {
        protected readonly T _factory;
        protected HttpClient _client;

        protected ComponentTestBase(T factory)
        {
            _factory = factory;
            _client = _factory.CreateDefaultClient();
            _client.DefaultRequestHeaders.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
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
