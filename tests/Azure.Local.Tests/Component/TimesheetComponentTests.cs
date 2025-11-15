using AwesomeAssertions;
using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.ApiService.Tests.Component.Setup;
using System.Net.Http.Json;

namespace Azure.Local.ApiService.Tests.Component
{
    public class TimesheetComponentTests : ComponentTestBase<ApiServiceWebApplicationFactoryBase>
    {
        private const string _endpoint = "/timesheet";

        public TimesheetComponentTests(ApiServiceWebApplicationFactoryBase factory) : base(factory)
        {
        }

        ~TimesheetComponentTests() => base.Dispose();

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
        public async Task AddEndpoint_ReturnsConflict_IfNotExists()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = GenerateAddTimesheetHttpRequest();
            await AddTestItemAsync(requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);
            request.Content = JsonContent.Create(requestBody);

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task PatchEndpoint_ReturnsBadRequest_WhenIdTooBig()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = GeneratePatchTimesheetHttpRequest();
            requestBody.Id = requestBody.Id.PadRight(300, 'X'); // Make the Id too big
            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint);
            request.Content = JsonContent.Create(requestBody);

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PatchEndpoint_ReturnsOk_IfAlreadyExists()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = GenerateAddTimesheetHttpRequest();
            await AddTestItemAsync(requestBody);

            requestBody.To = requestBody.To.AddDays(1); // Modify something

            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint);
            request.Content = JsonContent.Create(requestBody);

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PatchEndpoint_ReturnsFailure_IfNotExists()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = GenerateAddTimesheetHttpRequest();
            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint);
            request.Content = JsonContent.Create(requestBody);

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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

        [Fact]
        public async Task GetEndpoint_ReturnsFailure_WhenIdNotRecognised()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_endpoint}/{Guid.NewGuid().ToString()}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteEndpoint_ReturnsOk()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = GenerateAddTimesheetHttpRequest();
            await AddTestItemAsync(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_endpoint}/{requestBody.Id}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteEndpoint_ReturnsFailure_WhenIdNotRecognised()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_endpoint}/{Guid.NewGuid().ToString()}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private AddTimesheetHttpRequest GeneratePatchTimesheetHttpRequest()
            => GenerateAddTimesheetHttpRequest();

        private AddTimesheetHttpRequest GenerateAddTimesheetHttpRequest()
            => new AddTimesheetHttpRequest()
                {
                    Id = Guid.NewGuid().ToString(),
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
