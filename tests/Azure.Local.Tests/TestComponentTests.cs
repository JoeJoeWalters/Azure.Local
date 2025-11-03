using AwesomeAssertions;
using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.ApiService.Tests.Component.Setup;
using System.Net.Http.Json;

namespace Azure.Local.ApiService.Tests.Component
{
    public class TestComponentTests : ComponentTestBase
    {
        private const string _testId = "test-id-01";
        private const string _testName = "test-name-01";

        public TestComponentTests(ApiServiceWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task TestAddEndpoint_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/test");
            request.Content = JsonContent.Create(new AddTestItemHttpRequest()
            {
                Id = _testId,
                Name = _testName
            });

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task TestAddEndpoint_ReturnsBaadRequest_WhenIdTooBig()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/test");
            request.Content = JsonContent.Create(new AddTestItemHttpRequest()
            {
                Id = _testId.PadRight(101, '0'),
                Name = _testName
            });

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task TestAddEndpoint_ReturnsBaadRequest_WhenNameTooBig()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/test");
            request.Content = JsonContent.Create(new AddTestItemHttpRequest()
            {
                Id = _testId,
                Name = _testName.PadRight(101, '0')
            });

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
