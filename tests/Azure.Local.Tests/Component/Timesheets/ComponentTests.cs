using Azure.Local.ApiService.Tests.Component.Setup;
using Azure.Local.ApiService.Timesheets.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace Azure.Local.Tests.Component.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class ComponentTests(ApiServiceWebApplicationFactoryBase factory) : ComponentTestBase<ApiServiceWebApplicationFactoryBase>(factory)
    {
        private const string _endpoint = "/person/{personId}/timesheet/item";
        private const string _searchEndpoint = "/person/{personId}/timesheet/search";

        ~ComponentTests() => Dispose();

        [Fact]
        public async Task AddEndpoint_ReturnsOk()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint.Replace("{personId}", personId))
            {
                Content = JsonContent.Create(TestHelper.GenerateAddTimesheetHttpRequest(personId))
            };

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
            string personId = Guid.NewGuid().ToString();
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId);
            await TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", personId), requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint.Replace("{personId}", personId))
            {
                Content = JsonContent.Create(requestBody)
            };

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
            string personId = Guid.NewGuid().ToString();
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId);
            await TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", personId), requestBody);

            requestBody.Id = requestBody.Id.PadRight(300, 'X'); // Make the Id too big
            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint.Replace("{personId}", personId))
            {
                Content = JsonContent.Create(requestBody)
            };

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
            string personId = Guid.NewGuid().ToString();
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId);
            await TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", personId), requestBody);

            requestBody.To = requestBody.To.AddDays(1); // Modify something

            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint.Replace("{personId}", personId))
            {
                Content = JsonContent.Create(requestBody)
            };

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
            string personId = Guid.NewGuid().ToString();
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId);
            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint.Replace("{personId}", personId))
            {
                Content = JsonContent.Create(requestBody)
            };

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
            string personId = Guid.NewGuid().ToString();
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId);
            requestBody.Id = requestBody.Id.PadRight(300, 'X'); // Make the Id too big
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint.Replace("{personId}", personId))
            {
                Content = JsonContent.Create(requestBody)
            };

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
            string personId = Guid.NewGuid().ToString();
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId);
            await TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", personId), requestBody);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_endpoint.Replace("{personId}", personId)}/{requestBody.Id}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = response.GetTimesheetItem();
            result.Should().NotBeNull();
            result.Id.Should().Be(requestBody.Id);
            result.PersonId.Should().Be(personId);
        }

        [Fact]
        public async Task GetEndpoint_ReturnsFailure_WhenIdNotRecognised()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_endpoint.Replace("{personId}", personId)}/{Guid.NewGuid()}");
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
            string personId = Guid.NewGuid().ToString();
            AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId);
            await TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", personId), requestBody);
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_endpoint.Replace("{personId}", personId)}/{requestBody.Id}");
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
            string personId = Guid.NewGuid().ToString();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_endpoint.Replace("{personId}", personId)}/{Guid.NewGuid()}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SearchEndpoint_ReturnsCorrectRecords_WhenCalled()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            int timesheets = 7;
            List<string> timesheetIds = [];
            DateTime fromDate = DateTime.UtcNow;

            for (int t = 0; t < timesheets; t++)
            {
                AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(personId, fromDate.AddDays(t), fromDate.AddDays(t+1));
                timesheetIds.Add(requestBody.Id);
                await TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", personId), requestBody);
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_searchEndpoint.Replace("{personId}", personId)}?fromDate={fromDate.ToString("o")}&toDate={fromDate.AddDays(timesheets).ToString("o")}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = response.GetTimesheetItems();
            result.Should().NotBeNull();
            result.Should().HaveCount(timesheets);
            timesheetIds.ForEach(timesheetId => {
                result!.Any(t => t.Id == timesheetId).Should().BeTrue();
            });            
        }

    }
}
