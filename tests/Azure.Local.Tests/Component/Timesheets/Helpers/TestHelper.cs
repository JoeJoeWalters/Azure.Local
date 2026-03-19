using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.ApiService.Timesheets.Helpers;
using Azure.Local.Domain.Timesheets;
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
                CreatedBy = personId,
                Components =
                    [
                        new TimesheetHttpRequestComponent()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Units = 8.0,
                            From = from,
                            To = to,
                            TimeCode = Guid.NewGuid().ToString(),
                            ProjectCode = Guid.NewGuid().ToString(),
                            WorkType = "Regular",
                            IsBillable = true
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

        public static TimesheetResponse? GetTimesheetItem(this HttpResponseMessage? message)
        {
            try
            {
                return JsonSerializer.Deserialize<TimesheetResponse>(
                    message!.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                    jsonSerializerOptions);
            }
            catch
            {
                return null;
            }
        }
        public static List<TimesheetResponse>? GetTimesheetItems(this HttpResponseMessage? message)
        {
            try
            {
                return JsonSerializer.Deserialize<List<TimesheetResponse>>(
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
