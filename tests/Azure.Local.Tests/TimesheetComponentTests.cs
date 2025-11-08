using AwesomeAssertions;
using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.ApiService.Tests.Component.Setup;
using System.Net.Http.Json;

namespace Azure.Local.ApiService.Tests.Component
{
    public class TimesheetComponentTests : ComponentTestBase
    {
        private const string _testId = "test-id-01";
        private const string _endpoint = "/timesheet";
        //private const string _testName = "test-name-01";

        public TimesheetComponentTests(ApiServiceWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task AddEndpoint_ReturnsOk()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = GenerateAddTimesheetHttpRequest();
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);
            request.Content = JsonContent.Create(requestBody);

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddEndpoint_ReturnsBadRequest_WhenIdTooBig()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = GenerateAddTimesheetHttpRequest();
            requestBody.Id = requestBody.Id.PadRight(300, 'X'); // Make the Id too big
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);
            request.Content = JsonContent.Create(requestBody);

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /*
        [Fact]
        public async Task TestAddEndpoint_ReturnsBadRequest_WhenNameTooBig()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = GenerateAddTimesheetHttpRequest();
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);
            request.Content = JsonContent.Create(requestBody);

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        */

        [Fact]
        public async Task GetEndpoint_ReturnsOk()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = GenerateAddTimesheetHttpRequest();
            await AddTestItemAsync(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_endpoint}/{requestBody.Id}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private AddTimesheetHttpRequest GenerateAddTimesheetHttpRequest()
            => new AddTimesheetHttpRequest()
                {
                    Id = _testId,
                    From = DateTime.UtcNow,
                    To = DateTime.UtcNow.AddDays(1),
                    Components = new List<AddTimesheetHttpRequestComponent>()
                    {
                        new AddTimesheetHttpRequestComponent()
                        {
                            Units = 8.0,
                            From = DateTime.UtcNow,
                            To = DateTime.UtcNow.AddDays(1) 
                        }
                    }
                };

        private async Task<Boolean> AddTestItemAsync(AddTimesheetHttpRequest requestBody)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);
            request.Content = JsonContent.Create(requestBody);
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            var response = await _client.SendAsync(request, cancelToken);
            return (response.StatusCode == HttpStatusCode.OK);
        }
    }
}
