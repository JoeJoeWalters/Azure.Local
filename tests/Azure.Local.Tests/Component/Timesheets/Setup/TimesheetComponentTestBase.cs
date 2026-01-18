using Azure.Local.ApiService;
using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.ApiService.Timesheets.Helpers;
using Azure.Local.Tests.Component.Setup;
using Azure.Local.Tests.Component.Timesheets.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Azure.Local.Tests.Component.Timesheets.Setup
{
    [ExcludeFromCodeCoverage]
    public abstract class TimesheetComponentTestBase<T>(T factory) : ComponentTestBase<T>(factory) where T : WebApplicationFactory<Program>
    {
        private const string _endpoint = "/person/{personId}/timesheet/item";
        private const string _searchEndpoint = "/person/{personId}/timesheet/search";

        protected string _personId = string.Empty;
        protected string _timesheetId = string.Empty;
        protected List<string> _timesheetIds = [];
        protected AddTimesheetHttpRequest? _addRequestBody;
        protected PatchTimesheetHttpRequest? _patchRequestBody;
        protected HttpRequestMessage? _request;
        protected HttpResponseMessage? _response;

        ~TimesheetComponentTestBase()
        {
            Dispose();
        }

        protected async Task A_New_PersonId()
        {
            _personId = Guid.NewGuid().ToString();
            await Task.CompletedTask;
        }

        protected async Task A_Test_Timesheet_Is_Added()
        {
            _timesheetId = string.Empty;
            _addRequestBody = TestHelper.GenerateAddTimesheetHttpRequest(_personId);
            _addRequestBody.Should().NotBeNull();
            var addResult = TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", _personId), _addRequestBody).Result;
            addResult.Should().BeTrue();
            _timesheetId = _addRequestBody.Id;
            await Task.CompletedTask;
        }

        protected async Task A_Get_Request_Is_Performed(string timesheetId)
        {
            _request = new HttpRequestMessage(HttpMethod.Get, $"{_endpoint.Replace("{personId}", _personId)}/{timesheetId}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            _response = _client.SendAsync(_request, cancelToken).Result;
            await Task.CompletedTask;
        }

        protected async Task A_Patch_Request_Is_Performed()
            => A_Patch_Request_Is_Performed(Guid.NewGuid().ToString());

        protected async Task A_Patch_Request_Is_Performed(string? timesheetId)
        {
            _patchRequestBody = TestHelper.GeneratePatchTimesheetHttpRequest(_personId);
            _patchRequestBody.Should().NotBeNull();

            if (timesheetId != null)
            {
                _patchRequestBody.Id = timesheetId;
            }

            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint.Replace("{personId}", _personId))
            {
                Content = JsonContent.Create(_patchRequestBody)
            };
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            // Act
            _response = _client.SendAsync(request, cancelToken).Result;
            await Task.CompletedTask;
        }

        protected async Task A_Patch_Request_Is_Performed_On_Existing_The_Timesheet()
        {
            _patchRequestBody = _addRequestBody.ToPatchTimesheetHttpRequest();
            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint.Replace("{personId}", _personId))
            {
                Content = JsonContent.Create(_patchRequestBody)
            };

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            _response = _client.SendAsync(request, cancelToken).Result;
            await Task.CompletedTask;
        }

        protected async Task A_Delete_Request_Is_Performed(string timesheetId)
        {
            _request = new HttpRequestMessage(HttpMethod.Delete, $"{_endpoint.Replace("{personId}", _personId)}/{timesheetId}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            _response = _client.SendAsync(_request, cancelToken).Result;
            await Task.CompletedTask;
        }
        protected async Task An_Add_Request_Is_Performed_With_An_ExistingId(string timesheetId)
            => An_Add_Request_Is_Performed(timesheetId);

        protected async Task An_Add_Request_Is_Performed()
            => An_Add_Request_Is_Performed(null);

        protected async Task An_Add_Request_Is_Performed(string? timesheetId)
        {
            _addRequestBody = TestHelper.GenerateAddTimesheetHttpRequest(_personId);
            _addRequestBody.Should().NotBeNull();

            if (timesheetId != null)
            {
                _addRequestBody.Id = timesheetId;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint.Replace("{personId}", _personId))
            {
                Content = JsonContent.Create(_addRequestBody)
            };

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            _response = _client.SendAsync(request, cancelToken).Result;
            _timesheetId = _addRequestBody.Id;
            await Task.CompletedTask;
        }

        protected async Task The_Response_Should_Be(HttpStatusCode expectedStatusCode)
        {
            _response?.StatusCode.Should().Be(expectedStatusCode);
            await Task.CompletedTask;
        }

        protected async Task The_Timesheet_Should_Match(string timesheetId, string personId)
        {
            var result = _response.GetTimesheetItem();
            result.Should().NotBeNull();
            result.Id.Should().Be(timesheetId);
            result.PersonId.Should().Be(personId);
            await Task.CompletedTask;
        }

        protected async Task Multiple_Timesheets_Are_Added(DateTime from, int count)
        {
            _timesheetIds = [];
            for (int t = 0; t < count; t++)
            {
                AddTimesheetHttpRequest requestBody = TestHelper.GenerateAddTimesheetHttpRequest(_personId, from.AddDays(t), from.AddDays(t + 1));
                _timesheetIds.Add(requestBody.Id);
                _ = TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", _personId), requestBody).Result;
            }
            await Task.CompletedTask;
        }

        protected async Task A_Search_Request_Is_Performed(DateTime from, DateTime to)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_searchEndpoint.Replace("{personId}", _personId)}?fromDate={from:o}&toDate={to:o}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            _response = _client.SendAsync(request, cancelToken).Result;
            await Task.CompletedTask;
        }

        protected async Task Timesheets_Should_Be_Found(int count)
        {
            var result = _response.GetTimesheetItems();
            result.Should().NotBeNull();
            result.Should().HaveCount(count);

            var foundCount = 0;
            _timesheetIds.ForEach(timesheetId =>
            {
                foundCount += result!.Any(t => t.Id == timesheetId) ? 1 : 0;
            });
            foundCount.Should().Be(count);
            await Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
