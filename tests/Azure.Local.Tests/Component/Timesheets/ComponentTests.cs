using Azure.Local.ApiService.Tests.Component.Setup;
using Google.Protobuf.WellKnownTypes;
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

        [Scenario]
        public void SearchEndpoint_ReturnsCorrectRecords_WhenCalled()
        {
            DateTime timeStamp = DateTime.UtcNow;

            Runner.RunScenario
                (
                given => A_New_PersonId(),
                and => Multiple_Timesheets_Are_Added(timeStamp, 7),
                when => A_Search_Request_Is_Performed(timeStamp, timeStamp.AddDays(7)),
                then => The_Response_Should_Be(HttpStatusCode.OK),
                and => Timesheets_Should_Be_Found(7)
                );
        }
    }
}
