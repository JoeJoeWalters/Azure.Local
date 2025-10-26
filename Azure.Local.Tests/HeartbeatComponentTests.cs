using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Local.Tests
{
    public class HeartbeatComponentTests : ComponentTestBase
    {
        public HeartbeatComponentTests(ApiServiceWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task HeartbeatEndpoint_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/heartbeat");
         
            // Act
            var response = await _client.SendAsync(request);
            
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
    }
}
