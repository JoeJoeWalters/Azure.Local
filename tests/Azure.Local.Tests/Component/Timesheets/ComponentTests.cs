using Azure.Local.Domain.Timesheets;
using Azure.Local.Tests.Component.Setup;
using Azure.Local.Tests.Component.Timesheets.Setup;

namespace Azure.Local.Tests.Component.Timesheets
{
    [ExcludeFromCodeCoverage]
    [Collection("NoParallelization")]
    public class ComponentTests(ApiServiceWebApplicationFactoryBase factory) : TimesheetComponentTestBase<ApiServiceWebApplicationFactoryBase>(factory)
    {
        ~ComponentTests() => Dispose();

        [Fact]
        public async Task AddEndpoint_ReturnsOk()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    when => An_Add_Request_Is_Performed(),
                    then => The_Response_Should_Be(HttpStatusCode.OK)
                );
        }

        [Fact]
        public async Task AddEndpoint_ReturnsConflict_IfAlreadyExists()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    and => An_Add_Request_Is_Performed(),
                    when => An_Add_Request_Is_Performed_With_An_ExistingId(_timesheetId),
                    then => The_Response_Should_Be(HttpStatusCode.Conflict)
                );
        }

        [Fact]
        public async Task AddEndpoint_ReturnsBadRequest_WhenIdTooBig()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    when => An_Add_Request_Is_Performed(Guid.NewGuid().ToString().PadRight(300, 'X')),
                    then => The_Response_Should_Be(HttpStatusCode.BadRequest)
                );
        }


        [Fact]
        public async Task PatchEndpoint_ReturnsOk_IfAlreadyExists()
        {
            await ScenarioSteps.RunAsync(
                    given => A_New_PersonId(),
                    and => An_Add_Request_Is_Performed(),
                    when => A_Patch_Request_Is_Performed_On_Existing_The_Timesheet(),
                    then => The_Response_Should_Be(HttpStatusCode.OK)
                );
        }

        [Fact]
        public async Task PatchEndpoint_ReturnsFailure_IfNotExists()
        {
            await ScenarioSteps.RunAsync(
                    given => A_New_PersonId(),
                    when => A_Patch_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        [Fact]
        public async Task GetEndpoint_ReturnsOk()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    and => A_Test_Timesheet_Is_Added(),
                    when => A_Get_Request_Is_Performed(_timesheetId),
                    then => The_Response_Should_Be(HttpStatusCode.OK),
                    and => The_Timesheet_Should_Match(_timesheetId, _personId)
                );
        }

        [Fact]
        public async Task GetEndpoint_ReturnsNotFound_WhenIdNotRecognised()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    when => A_Get_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        [Fact]
        public async Task RenderEndpoint_ReturnsHtml()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    and => A_Test_Timesheet_Is_Added(),
                    when => A_Render_Request_Is_Performed(_timesheetId),
                    then => The_Response_Should_Be(HttpStatusCode.OK),
                    and => The_Rendered_Html_Should_Contain($"Timesheet {_timesheetId}")
                );
        }

        [Fact]
        public async Task RenderEndpoint_ReturnsPdf()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    and => A_Test_Timesheet_Is_Added(),
                    when => A_Render_Request_Is_Performed(_timesheetId, "pdf"),
                    then => The_Response_Should_Be(HttpStatusCode.OK),
                    and => The_Rendered_Pdf_Should_Be_Returned()
                );
        }

        [Fact]
        public async Task RenderEndpoint_ReturnsNotFound_WhenIdNotRecognised()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    when => A_Render_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        [Fact]
        public async Task RenderEndpoint_ReturnsBadRequest_WhenOutputTypeInvalid()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    and => A_Test_Timesheet_Is_Added(),
                    when => A_Render_Request_Is_Performed(_timesheetId, "invalid-format"),
                    then => The_Response_Should_Be(HttpStatusCode.BadRequest)
                );
        }

        [Fact]
        public async Task DeleteEndpoint_ReturnsOk()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                when => A_Delete_Request_Is_Performed(_timesheetId),
                then => The_Response_Should_Be(HttpStatusCode.OK)
                );
        }

        [Fact]
        public async Task DeleteEndpoint_ReturnsNotFound_WhenIdNotRecognised()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                when => A_Delete_Request_Is_Performed(Guid.NewGuid().ToString()),
                then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        [Fact]
        public async Task SearchEndpoint_ReturnsCorrectRecords_WhenCalled()
        {
            DateTime timeStamp = DateTime.UtcNow;

            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                and => Multiple_Timesheets_Are_Added(timeStamp, 7),
                when => A_Search_Request_Is_Performed(timeStamp, timeStamp.AddDays(7)),
                then => The_Response_Should_Be(HttpStatusCode.OK),
                and => Timesheets_Should_Be_Found(7)
                );
        }

        [Fact]
        public async Task StateChangeEndpoint_Submit_ReturnsOk()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                then => The_Response_Should_Be(HttpStatusCode.OK),
                and => The_Response_Should_Contain_Message("submitted successfully")
                );
        }

        [Fact]
        public async Task StateChangeEndpoint_Approve_ReturnsBadRequest_ForSelfApproval()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                and => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Approve),
                then => The_Response_Should_Be(HttpStatusCode.BadRequest),
                and => The_Response_Should_Contain_Message("cannot approve your own timesheet")
                );
        }

        [Fact]
        public async Task StateChangeEndpoint_Reject_ReturnsOk_WithReason()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                and => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Reject, "Invalid hours"),
                then => The_Response_Should_Be(HttpStatusCode.OK),
                and => The_Response_Should_Contain_Message("rejected")
                );
        }

        [Fact]
        public async Task StateChangeEndpoint_Reject_ReturnsBadRequest_WithoutReason()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                and => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Reject),
                then => The_Response_Should_Be(HttpStatusCode.BadRequest)
                );
        }

        [Fact]
        public async Task StateChangeEndpoint_Recall_ReturnsOk()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                and => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Recall),
                then => The_Response_Should_Be(HttpStatusCode.OK),
                and => The_Response_Should_Contain_Message("recalled")
                );
        }

        [Fact]
        public async Task StateChangeEndpoint_ReturnsNotFound_WhenTimesheetDoesNotExist()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                then => The_Response_Should_Be(HttpStatusCode.NotFound)
                );
        }

        [Fact]
        public async Task StateChangeEndpoint_ReturnsBadRequest_WhenInvalidStateTransition()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                and => A_Test_Timesheet_Is_Added(),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Approve),
                then => The_Response_Should_Be(HttpStatusCode.BadRequest)
                );
        }
    }
}
