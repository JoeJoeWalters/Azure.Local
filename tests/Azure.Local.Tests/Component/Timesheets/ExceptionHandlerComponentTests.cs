using Azure.Local.ApiService.Tests.Component.Setup;
using Azure.Local.ApiService.Tests.Component.Timesheets.Setup;
using Azure.Local.ApiService.Timesheets.Contracts;
using System.Net.Http.Json;

namespace Azure.Local.Tests.Component.Timesheets
{
    public class ExceptionHandlerComponentTests(ApiServiceWebApplicationFactoryExceptionHandling factory) : ComponentTestBase<ApiServiceWebApplicationFactoryExceptionHandling>(factory)
    {
        private const string _endpoint = "/timesheet";

        ~ExceptionHandlerComponentTests() => Dispose();

        [Fact]
        public async Task AddEndpoint_ReturnsInternalServerError()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest();
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = JsonContent.Create(requestBody)
            };

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task PatchEndpoint_ReturnsInternalServerError()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = TestHelper.GeneratePatchTimesheetHttpRequest();
            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint)
            {
                Content = JsonContent.Create(requestBody)
            };

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task GetEndpoint_ReturnsInternalServerError()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest();
            await TestHelper.AddTestItemAsync(_client, _endpoint, requestBody);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_endpoint}/{requestBody.Id}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task DeleteEndpoint_ReturnsInternalServerError()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest();
            await TestHelper.AddTestItemAsync(_client, _endpoint, requestBody);
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_endpoint}/{requestBody.Id}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
