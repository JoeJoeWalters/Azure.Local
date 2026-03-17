using Azure.Local.Tests.Component.Setup;
using Azure.Local.ApiService.Versioning;

namespace Azure.Local.Tests.Component.Heartbeat
{
    public class ComponentTests(ApiServiceWebApplicationFactoryBase factory) : ComponentTestBase<ApiServiceWebApplicationFactoryBase>(factory)
    {
        [Fact]
        public async Task HeartbeatEndpoint_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/heartbeat");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task HeartbeatEndpoint_ReturnsBadRequest_ForUnsupportedApiVersion()
        {
            // Arrange
            using var client = _factory.CreateDefaultClient();
            client.DefaultRequestHeaders.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add(ApiVersioningConstants.HeaderName, "999.0"); // Using an very large unsupported API version to trigger BadRequest response

            var request = new HttpRequestMessage(HttpMethod.Get, "/heartbeat");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
