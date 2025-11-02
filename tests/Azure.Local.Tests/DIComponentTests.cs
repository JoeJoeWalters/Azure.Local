using AwesomeAssertions;
using Azure.Local.ApiService.Tests.Component.Setup;

namespace Azure.Local.ApiService.Tests.Component
{
    public class DIComponentTests : ComponentTestBase
    {
        public DIComponentTests(ApiServiceWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task TestAddEndpoint_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/test");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task TestGetEndpoint_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/test");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
