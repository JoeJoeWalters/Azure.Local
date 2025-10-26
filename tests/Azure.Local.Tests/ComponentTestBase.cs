using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Local.Tests
{
    public abstract class ComponentTestBase : IClassFixture<ApiServiceWebApplicationFactory>, IDisposable
    {
        protected readonly ApiServiceWebApplicationFactory _factory;
        protected HttpClient _client;

        protected ComponentTestBase(ApiServiceWebApplicationFactory factory)
        {
            _factory = factory;
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
