using Azure.Local.ApiService.Test.Contracts;
using System.Net.Http.Json;

namespace Azure.Local.ApiService.Tests.Component.Timesheets
{
    public static class TestHelper
    {
        public static AddTimesheetHttpRequest GeneratePatchTimesheetHttpRequest()
            => GenerateAddTimesheetHttpRequest();

        public static AddTimesheetHttpRequest GenerateAddTimesheetHttpRequest()
            => new AddTimesheetHttpRequest()
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = Guid.NewGuid().ToString(),
                From = DateTime.UtcNow,
                To = DateTime.UtcNow.AddDays(1),
                Components = new List<AddTimesheetHttpRequestComponent>()
                    {
                        new AddTimesheetHttpRequestComponent()
                        {
                            Units = 8.0,
                            From = DateTime.UtcNow,
                            To = DateTime.UtcNow.AddDays(1),
                            Code = Guid.NewGuid().ToString()
                        }
                    }
            };

        public static async Task<bool> AddTestItemAsync(HttpClient httpClient, string endpoint, AddTimesheetHttpRequest requestBody)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Content = JsonContent.Create(requestBody);
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            var response = await httpClient.SendAsync(request, cancelToken);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
