using Azure.Local.Tests.Component.Setup;
using Azure.Local.ApiService.Versioning;
using Azure.Local.ApiService.Timesheets.Helpers;

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
            var _clonedClient = ClonedHttpClient(); // Don't modify the original client as it might be used by other tests
            _clonedClient.DefaultRequestHeaders.Remove(ApiVersioningConstants.HeaderName); // Remove any existing API version header
            _clonedClient.DefaultRequestHeaders.Add(ApiVersioningConstants.HeaderName, "999.0"); // Using an very large unsupported API version to trigger BadRequest response

            var request = new HttpRequestMessage(HttpMethod.Get, "/heartbeat");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _clonedClient.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            _clonedClient.Dispose(); // Dispose the cloned client after use
        }
    }
}
