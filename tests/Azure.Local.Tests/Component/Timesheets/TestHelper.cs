using Azure.Local.ApiService.Timesheets.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace Azure.Local.Tests.Component.Timesheets
{
    [ExcludeFromCodeCoverage]
    public static class TestHelper
    {
        public static AddTimesheetHttpRequest GeneratePatchTimesheetHttpRequest()
            => GenerateAddTimesheetHttpRequest();

        public static AddTimesheetHttpRequest GeneratePatchTimesheetHttpRequest(string personId)
            => GenerateAddTimesheetHttpRequest(personId);

        public static AddTimesheetHttpRequest GenerateAddTimesheetHttpRequest()
            => GenerateAddTimesheetHttpRequest(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

        public static AddTimesheetHttpRequest GenerateAddTimesheetHttpRequest(string personId)
            => GenerateAddTimesheetHttpRequest(personId, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

        public static AddTimesheetHttpRequest GenerateAddTimesheetHttpRequest(string personId, DateTime from, DateTime to)
            => new()
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = personId,
                From = from,
                To = to,
                Components =
                    [
                        new AddTimesheetHttpRequestComponent()
                        {
                            Units = 8.0,
                            From = from,
                            To = to,
                            Code = Guid.NewGuid().ToString()
                        }
                    ]
            };

        public static async Task<bool> AddTestItemAsync(HttpClient httpClient, string endpoint, AddTimesheetHttpRequest requestBody)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = JsonContent.Create(requestBody)
            };
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            var response = await httpClient.SendAsync(request, cancelToken);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
