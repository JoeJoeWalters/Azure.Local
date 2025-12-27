using Azure.Local.ApiService.Tests.Component.Setup;
using Azure.Local.ApiService.Tests.Component.Timesheets.Setup;
using Azure.Local.ApiService.Timesheets.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace Azure.Local.Tests.Component.Timesheets
{
    public class ExceptionHandlerComponentTests(ApiServiceWebApplicationFactoryExceptionHandling factory) : ComponentTestBase<ApiServiceWebApplicationFactoryExceptionHandling>(factory)
    {
        private const string _endpoint = "/person/{personId}/timesheet/item";
        private const string _searchEndpoint = "/person/{personId}/timesheet/search";

        ~ExceptionHandlerComponentTests() => Dispose();

        [Fact]
        public async Task AddEndpoint_ReturnsInternalServerError()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId);
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint.Replace("{personId}", personId))
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
            string personId = Guid.NewGuid().ToString();
            PatchTimesheetHttpRequest requestBody = TestHelper.GeneratePatchTimesheetHttpRequest(personId);
            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint.Replace("{personId}", personId))
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
            string personId = Guid.NewGuid().ToString();
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId);
            await TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", personId), requestBody);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_endpoint.Replace("{personId}", personId)}/{requestBody.Id}");
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
            string personId = Guid.NewGuid().ToString();
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId);
            await TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", personId), requestBody);
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_endpoint.Replace("{personId}", personId)}/{requestBody.Id}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task SearchEndpoint_ReturnsInternalServerError()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId);
            await TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", personId), requestBody);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_searchEndpoint.Replace("{personId}", personId)}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
