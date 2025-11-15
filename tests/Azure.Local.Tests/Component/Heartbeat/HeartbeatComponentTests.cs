using AwesomeAssertions;
using Azure.Local.ApiService.Tests.Component.Setup;

namespace Azure.Local.ApiService.Tests.Component.Heartbeat
{
    public class HeartbeatComponentTests : ComponentTestBase<ApiServiceWebApplicationFactoryBase>
    {
        public HeartbeatComponentTests(ApiServiceWebApplicationFactoryBase factory) : base(factory)
        {
        }

        [Fact] // Explicitly qualify the attribute to avoid ambiguity
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
