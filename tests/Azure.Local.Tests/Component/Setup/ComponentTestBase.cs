using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.Tests.Component.Timesheets;
using LightBDD.XUnit2;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Azure.Local.ApiService.Test.Helpers;

[assembly: LightBddScope]
namespace Azure.Local.ApiService.Tests.Component.Setup
{
    public abstract class ComponentTestBase<T> : FeatureFixture, IClassFixture<T>, IDisposable where T: WebApplicationFactory<Program>
    {
        protected readonly T _factory;
        protected HttpClient _client;

        private const string _endpoint = "/person/{personId}/timesheet/item";
        private const string _searchEndpoint = "/person/{personId}/timesheet/search";

        protected string _personId = string.Empty;
        protected string _timesheetId = string.Empty;
        protected AddTimesheetHttpRequest _addRequestBody;
        protected PatchTimesheetHttpRequest _patchRequestBody;
        protected HttpRequestMessage _request;
        protected HttpResponseMessage _response;

        protected ComponentTestBase(T factory)
        {
            _factory = factory;
            //_factory.WithWebHostBuilder(builder =>
            //{
            //});
            _client = _factory.CreateDefaultClient();
            _client.DefaultRequestHeaders.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        ~ComponentTestBase()
        {
            Dispose();
        }


        protected void A_New_PersonId()
        {
            _personId = Guid.NewGuid().ToString();
        }

        protected void A_Test_Timesheet_Is_Added()
        {
            _timesheetId = string.Empty;
            _addRequestBody = TestHelper.GenerateAddTimesheetHttpRequest(_personId);
            var addResult = TestHelper.AddTestItemAsync(_client, _endpoint.Replace("{personId}", _personId), _addRequestBody).Result;
            addResult.Should().BeTrue();
            _timesheetId = _addRequestBody.Id;
        }

        protected void A_Get_Request_Is_Performed(string timesheetId)
        {
            _request = new HttpRequestMessage(HttpMethod.Get, $"{_endpoint.Replace("{personId}", _personId)}/{timesheetId}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            _response = _client.SendAsync(_request, cancelToken).Result;
        }

        protected void A_Patch_Request_Is_Performed()
            => A_Patch_Request_Is_Performed(Guid.NewGuid().ToString());

        protected void A_Patch_Request_Is_Performed(string? timesheetId)
        {
            _patchRequestBody = TestHelper.GeneratePatchTimesheetHttpRequest(_personId);
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
        }

        protected void A_Patch_Request_Is_Performed_On_Existing_The_Timesheet()
        {
            _patchRequestBody = _addRequestBody.ToPatchTimesheetHttpRequest();
            var request = new HttpRequestMessage(HttpMethod.Patch, _endpoint.Replace("{personId}", _personId))
            {
                Content = JsonContent.Create(_patchRequestBody)
            };

            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            // Act
            _response = _client.SendAsync(request, cancelToken).Result;
        }

        protected void A_Delete_Request_Is_Performed(string timesheetId)
        {
            _request = new HttpRequestMessage(HttpMethod.Delete, $"{_endpoint.Replace("{personId}", _personId)}/{timesheetId}");
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            _response = _client.SendAsync(_request, cancelToken).Result;
        }
        protected void An_Add_Request_Is_Performed_With_An_ExistingId(string timesheetId)
            => An_Add_Request_Is_Performed(timesheetId);

        protected void An_Add_Request_Is_Performed() 
            => An_Add_Request_Is_Performed(null);

        protected void An_Add_Request_Is_Performed(string? timesheetId)
        {
            _addRequestBody = TestHelper.GenerateAddTimesheetHttpRequest(_personId);
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
        }

        protected void The_Response_Should_Be(HttpStatusCode expectedStatusCode)
        {
            _response.StatusCode.Should().Be(expectedStatusCode);
        }

        protected void The_Timesheet_Should_Match(string timesheetId, string personId)
        {
            var result = _response.GetTimesheetItem();
            result.Should().NotBeNull();
            result.Id.Should().Be(timesheetId);
            result.PersonId.Should().Be(personId);
        }

        public void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
