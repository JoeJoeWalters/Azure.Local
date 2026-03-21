using Azure.Local.Domain.Timesheets;
using Azure.Local.Tests.Component.Setup;
using Azure.Local.Tests.Component.Timesheets.Setup;

namespace Azure.Local.Tests.Component.Timesheets
{
    [ExcludeFromCodeCoverage]
    [Collection("NoParallelization")]
    public class ExceptionHandlerComponentTests(ApiServiceWebApplicationFactoryExceptionHandling factory) : TimesheetComponentTestBase<ApiServiceWebApplicationFactoryExceptionHandling>(factory)
    {
        ~ExceptionHandlerComponentTests() => Dispose();

        [Fact]
        public async Task AddEndpoint_ReturnsInternalServerError()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    when => An_Add_Request_Is_Performed(),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Fact]
        public async Task PatchEndpoint_ReturnsInternalServerError()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    when => A_Patch_Request_Is_Performed(),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Fact]
        public async Task GetEndpoint_ReturnsInternalServerError()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    when => A_Get_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Fact]
        public async Task RenderEndpoint_ReturnsInternalServerError()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    when => A_Render_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Fact]
        public async Task DeleteEndpoint_ReturnsInternalServerError()
        {
            await ScenarioSteps.RunAsync
                (
                    given => A_New_PersonId(),
                    when => A_Delete_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Fact]
        public async Task SearchEndpoint_ReturnsInternalServerError()
        {
            DateTime timeStamp = DateTime.UtcNow;

            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                when => A_Search_Request_Is_Performed(timeStamp, timeStamp.AddDays(7)),
                then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Fact]
        public async Task StateChangeEndpoint_Submit_ReturnsInternalServerError()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Submit),
                then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Fact]
        public async Task StateChangeEndpoint_Approve_ReturnsInternalServerError()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Approve),
                then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Fact]
        public async Task StateChangeEndpoint_Reject_ReturnsInternalServerError()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Reject, "Test reason"),
                then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Fact]
        public async Task StateChangeEndpoint_Recall_ReturnsInternalServerError()
        {
            await ScenarioSteps.RunAsync
                (
                given => A_New_PersonId(),
                when => A_ChangeState_Request_Is_Performed(TimesheetStateAction.Recall),
                then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }
    }
}
