using Azure.Local.ApiService.Test.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Local.ApiService.Tests.Component
{
    public static class TimesheetComponentTestHelper
    {
        public static AddTimesheetHttpRequest GeneratePatchTimesheetHttpRequest()
            => GenerateAddTimesheetHttpRequest();

        public static AddTimesheetHttpRequest GenerateAddTimesheetHttpRequest()
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

        public static async Task<Boolean> AddTestItemAsync(HttpClient httpClient, string endpoint, AddTimesheetHttpRequest requestBody)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Content = JsonContent.Create(requestBody);
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            var response = await httpClient.SendAsync(request, cancelToken);
            return (response.StatusCode == HttpStatusCode.OK);
        }
    }
}
