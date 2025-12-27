using Azure.Local.ApiService.Tests.Component.Setup;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using Microsoft.Azure.Cosmos;

namespace Azure.Local.Tests.Component.Timesheets
{
    public class ComponentTests(ApiServiceWebApplicationFactoryBase factory) : ComponentTestBase<ApiServiceWebApplicationFactoryBase>(factory)
    {
        ~ComponentTests() => Dispose();

        [Scenario]
        public async Task AddEndpoint_ReturnsOk()
        {
            Runner.RunScenario
                (
                    given => A_New_PersonId(),
                    when => An_Add_Request_Is_Performed(),
                    then => The_Response_Should_Be(HttpStatusCode.OK)
                );
        }

        [Scenario]
        public async Task AddEndpoint_ReturnsConflict_IfAlreadyExists()
        {
            Runner.RunScenario
                (
                    given => A_New_PersonId(),
                    and => An_Add_Request_Is_Performed(),
                    when => An_Add_Request_Is_Performed_With_An_ExistingId(_timesheetId),
                    then => The_Response_Should_Be(HttpStatusCode.Conflict)
                );
        }

        [Scenario]
        public async Task AddEndpoint_ReturnsBadRequest_WhenIdTooBig()
        {
            Runner.RunScenario
                (
                    given => A_New_PersonId(),
                    when => An_Add_Request_Is_Performed(Guid.NewGuid().ToString().PadRight(300, 'X')),
                    then => The_Response_Should_Be(HttpStatusCode.BadRequest)
                );
        }


        [Scenario]
        public async Task PatchEndpoint_ReturnsOk_IfAlreadyExists()
        {
            Runner.RunScenario(
                    given => A_New_PersonId(),
                    and => An_Add_Request_Is_Performed(),
                    when => A_Patch_Request_Is_Performed_On_Existing_The_Timesheet(),
                    then => The_Response_Should_Be(HttpStatusCode.OK)
                );
        }

        [Scenario]
        public async Task PatchEndpoint_ReturnsFailure_IfNotExists()
        {
            Runner.RunScenario(
                    given => A_New_PersonId(),
                    when => A_Patch_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        [Scenario]
        public async Task GetEndpoint_ReturnsOk()
        {
            Runner.RunScenario
                (
                    given => A_New_PersonId(),
                    and => A_Test_Timesheet_Is_Added(),
                    when => A_Get_Request_Is_Performed(_timesheetId),
                    then => The_Response_Should_Be(HttpStatusCode.OK),
                    and => The_Timesheet_Should_Match(_timesheetId, _personId)
                );
        }

        [Scenario]
        public void GetEndpoint_ReturnsNotFound_WhenIdNotRecognised()
        {
            Runner.RunScenario
                (
                    given => A_New_PersonId(),
                    when => A_Get_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        [Scenario]
        public void DeleteEndpoint_ReturnsOk()
        {
            Runner.RunScenario
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                when => A_Delete_Request_Is_Performed(_timesheetId),
                then => The_Response_Should_Be(HttpStatusCode.OK)
                );
        }

        [Scenario]
        public void DeleteEndpoint_ReturnsNotFound_WhenIdNotRecognised()
        {
            Runner.RunScenario
                (
                given => A_New_PersonId(),
                when => A_Delete_Request_Is_Performed(Guid.NewGuid().ToString()),
                then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        /*
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
        */
    }
}
