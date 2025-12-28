using Azure.Local.Tests.Component.Setup;

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
    }
}
