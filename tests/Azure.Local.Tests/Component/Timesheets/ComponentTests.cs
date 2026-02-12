using Azure.Local.Domain.Timesheets;
using Azure.Local.Tests.Component.Setup;
using Azure.Local.Tests.Component.Timesheets.Setup;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace Azure.Local.Tests.Component.Timesheets
{
    [ExcludeFromCodeCoverage]
    [Collection("NoParallelization")]
    public class ComponentTests(ApiServiceWebApplicationFactoryBase factory) : TimesheetComponentTestBase<ApiServiceWebApplicationFactoryBase>(factory)
    {
        ~ComponentTests() => Dispose();

        [Scenario]
        public async Task AddEndpoint_ReturnsOk()
        {
            _ = Runner.RunScenarioAsync
                (
                    given => A_New_PersonId(),
                    when => An_Add_Request_Is_Performed(),
                    then => The_Response_Should_Be(HttpStatusCode.OK)
                );
        }

        [Scenario]
        public async Task AddEndpoint_ReturnsConflict_IfAlreadyExists()
        {
            _ = Runner.RunScenarioAsync
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
            _ = Runner.RunScenarioAsync
                (
                    given => A_New_PersonId(),
                    when => An_Add_Request_Is_Performed(Guid.NewGuid().ToString().PadRight(300, 'X')),
                    then => The_Response_Should_Be(HttpStatusCode.BadRequest)
                );
        }


        [Scenario]
        public async Task PatchEndpoint_ReturnsOk_IfAlreadyExists()
        {
            _ = Runner.RunScenarioAsync(
                    given => A_New_PersonId(),
                    and => An_Add_Request_Is_Performed(),
                    when => A_Patch_Request_Is_Performed_On_Existing_The_Timesheet(),
                    then => The_Response_Should_Be(HttpStatusCode.OK)
                );
        }

        [Scenario]
        public async Task PatchEndpoint_ReturnsFailure_IfNotExists()
        {
            _ = Runner.RunScenarioAsync(
                    given => A_New_PersonId(),
                    when => A_Patch_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        [Scenario]
        public async Task GetEndpoint_ReturnsOk()
        {
            _ = Runner.RunScenarioAsync
                (
                    given => A_New_PersonId(),
                    and => A_Test_Timesheet_Is_Added(),
                    when => A_Get_Request_Is_Performed(_timesheetId),
                    then => The_Response_Should_Be(HttpStatusCode.OK),
                    and => The_Timesheet_Should_Match(_timesheetId, _personId)
                );
        }

        [Scenario]
        public async Task GetEndpoint_ReturnsNotFound_WhenIdNotRecognised()
        {
            _ = Runner.RunScenarioAsync
                (
                    given => A_New_PersonId(),
                    when => A_Get_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        [Scenario]
        public async Task DeleteEndpoint_ReturnsOk()
        {
            _ = Runner.RunScenarioAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                when => A_Delete_Request_Is_Performed(_timesheetId),
                then => The_Response_Should_Be(HttpStatusCode.OK)
                );
        }

        [Scenario]
        public async Task DeleteEndpoint_ReturnsNotFound_WhenIdNotRecognised()
        {
            _ = Runner.RunScenarioAsync
                (
                given => A_New_PersonId(),
                when => A_Delete_Request_Is_Performed(Guid.NewGuid().ToString()),
                then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        [Scenario]
        public async Task SearchEndpoint_ReturnsCorrectRecords_WhenCalled()
        {
            DateTime timeStamp = DateTime.UtcNow;

            _ = Runner.RunScenarioAsync
                (
                given => A_New_PersonId(),
                and => Multiple_Timesheets_Are_Added(timeStamp, 7),
                when => A_Search_Request_Is_Performed(timeStamp, timeStamp.AddDays(7)),
                then => The_Response_Should_Be(HttpStatusCode.OK),
                and => Timesheets_Should_Be_Found(7)
                );
        }

        [Scenario]
        public async Task StateChangeEndpoint_Submit_ReturnsOk()
        {
            _ = Runner.RunScenarioAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                then => The_Response_Should_Be(HttpStatusCode.OK),
                and => The_Response_Should_Contain_Message("submitted successfully")
                );
        }

        [Scenario]
        public async Task StateChangeEndpoint_Approve_ReturnsOk()
        {
            _ = Runner.RunScenarioAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                and => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Approve),
                then => The_Response_Should_Be(HttpStatusCode.OK),
                and => The_Response_Should_Contain_Message("approved successfully")
                );
        }

        [Scenario]
        public async Task StateChangeEndpoint_Reject_ReturnsOk_WithReason()
        {
            _ = Runner.RunScenarioAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                and => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Reject, "Invalid hours"),
                then => The_Response_Should_Be(HttpStatusCode.OK),
                and => The_Response_Should_Contain_Message("rejected")
                );
        }

        [Scenario]
        public async Task StateChangeEndpoint_Reject_ReturnsBadRequest_WithoutReason()
        {
            _ = Runner.RunScenarioAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                and => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Reject),
                then => The_Response_Should_Be(HttpStatusCode.BadRequest)
                );
        }

        [Scenario]
        public async Task StateChangeEndpoint_Recall_ReturnsOk()
        {
            _ = Runner.RunScenarioAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                and => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Recall),
                then => The_Response_Should_Be(HttpStatusCode.OK),
                and => The_Response_Should_Contain_Message("recalled successfully")
                );
        }

        [Scenario]
        public async Task StateChangeEndpoint_ReturnsNotFound_WhenTimesheetDoesNotExist()
        {
            _ = Runner.RunScenarioAsync
                (
                given => A_New_PersonId(),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        [Scenario]
        public async Task StateChangeEndpoint_ReturnsBadRequest_WhenInvalidStateTransition()
        {
            _ = Runner.RunScenarioAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Approve),
                then => The_Response_Should_Be(HttpStatusCode.BadRequest)
                );
        }
    }
}
