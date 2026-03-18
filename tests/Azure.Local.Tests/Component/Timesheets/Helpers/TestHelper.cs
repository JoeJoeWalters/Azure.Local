using Azure.Local.ApiService.Timesheets.Contracts.V1;
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

        public static PatchTimesheetHttpRequestV1? GeneratePatchTimesheetHttpRequest()
            => GenerateAddTimesheetHttpRequest().ToPatchTimesheetHttpRequest();

        public static PatchTimesheetHttpRequestV1? GeneratePatchTimesheetHttpRequest(AddTimesheetHttpRequestV1 addRequest)
            => addRequest.ToPatchTimesheetHttpRequest();

        public static PatchTimesheetHttpRequestV1? GeneratePatchTimesheetHttpRequest(string personId)
            => GenerateAddTimesheetHttpRequest(personId).ToPatchTimesheetHttpRequest();

        public static AddTimesheetHttpRequestV1? GenerateAddTimesheetHttpRequest()
            => GenerateAddTimesheetHttpRequest(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

        public static AddTimesheetHttpRequestV1? GenerateAddTimesheetHttpRequest(string personId)
            => GenerateAddTimesheetHttpRequest(personId, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

        public static AddTimesheetHttpRequestV1 GenerateAddTimesheetHttpRequest(string personId, DateTime from, DateTime to)
            => new()
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = personId,
                From = from,
                To = to,
                CreatedBy = personId,
                Components =
                    [
                        new TimesheetHttpRequestComponentV1()
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

        public static async Task<bool> AddTestItemAsync(HttpClient httpClient, string endpoint, AddTimesheetHttpRequestV1 requestBody)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = JsonContent.Create(requestBody)
            };
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            var response = await httpClient.SendAsync(request, cancelToken);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public static TimesheetResponseV1? GetTimesheetItem(this HttpResponseMessage? message)
        {
            try
            {
                return JsonSerializer.Deserialize<TimesheetResponseV1>(
                    message!.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                    jsonSerializerOptions);
            }
            catch
            {
                return null;
            }
        }
        public static List<TimesheetResponseV1>? GetTimesheetItems(this HttpResponseMessage? message)
        {
            try
            {
                return JsonSerializer.Deserialize<List<TimesheetResponseV1>>(
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
