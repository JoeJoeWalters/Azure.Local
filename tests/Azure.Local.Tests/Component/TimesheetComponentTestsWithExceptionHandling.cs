using AwesomeAssertions;
using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.ApiService.Tests.Component.Fakes.Applications;
using Azure.Local.ApiService.Tests.Component.Setup;
using Azure.Local.Application.Timesheets;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Json;

namespace Azure.Local.ApiService.Tests.Component
{
    public class TimesheetComponentTestsWithExceptionHandling : ComponentTestBase<ApiServiceWebApplicationFactoryExceptionHandling>
    {
        private const string _endpoint = "/timesheet";

        public TimesheetComponentTestsWithExceptionHandling(ApiServiceWebApplicationFactoryExceptionHandling factory) : base(factory)
        {
        }

        ~TimesheetComponentTestsWithExceptionHandling() => base.Dispose();

        [Fact]
        public async Task AddEndpoint_ReturnsInternalServerError()
        {
            // Arrange
            AddTimesheetHttpRequest requestBody = GenerateAddTimesheetHttpRequest();
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);
            request.Content = JsonContent.Create(requestBody);

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
            AddTimesheetHttpRequest requestBody = GeneratePatchTimesheetHttpRequest();
            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint);
            request.Content = JsonContent.Create(requestBody);

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
            AddTimesheetHttpRequest requestBody = GenerateAddTimesheetHttpRequest();
            await AddTestItemAsync(requestBody);
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
            AddTimesheetHttpRequest requestBody = GenerateAddTimesheetHttpRequest();
            await AddTestItemAsync(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_endpoint}/{requestBody.Id}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            var response = await _client.SendAsync(request, cancelToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
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
