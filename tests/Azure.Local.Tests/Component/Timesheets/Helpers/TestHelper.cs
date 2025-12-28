using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.ApiService.Timesheets.Helpers;
using Azure.Local.Domain.Timesheets;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json;

namespace Azure.Local.Tests.Component.Timesheets.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class TestHelper
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static PatchTimesheetHttpRequest? GeneratePatchTimesheetHttpRequest()
            => GenerateAddTimesheetHttpRequest().ToPatchTimesheetHttpRequest();

        public static PatchTimesheetHttpRequest? GeneratePatchTimesheetHttpRequest(AddTimesheetHttpRequest addRequest)
            => addRequest.ToPatchTimesheetHttpRequest();

        public static PatchTimesheetHttpRequest? GeneratePatchTimesheetHttpRequest(string personId)
            => GenerateAddTimesheetHttpRequest(personId).ToPatchTimesheetHttpRequest();

        public static AddTimesheetHttpRequest? GenerateAddTimesheetHttpRequest()
            => GenerateAddTimesheetHttpRequest(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

        public static AddTimesheetHttpRequest? GenerateAddTimesheetHttpRequest(string personId)
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
                        new TimesheetHttpRequestComponent()
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

        public static TimesheetItem? GetTimesheetItem(this HttpResponseMessage? message)
        {
            try
            {
                return JsonSerializer.Deserialize<TimesheetItem>(
                    message!.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                    jsonSerializerOptions);
            }
            catch
            {
                return null;
            }
        }
        public static List<TimesheetItem>? GetTimesheetItems(this HttpResponseMessage? message)
        {
            try
            {
                return JsonSerializer.Deserialize<List<TimesheetItem>>(
                    message!.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                    jsonSerializerOptions);
            }
            catch
            {
                return null;
            }
        }
    }
}
