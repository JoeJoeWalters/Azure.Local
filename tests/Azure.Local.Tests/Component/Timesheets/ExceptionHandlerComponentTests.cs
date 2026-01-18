using Azure.Local.Tests.Component.Timesheets.Setup;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace Azure.Local.Tests.Component.Timesheets
{
    [ExcludeFromCodeCoverage]
    [Collection("NoParallelization")]
    public class ExceptionHandlerComponentTests(ApiServiceWebApplicationFactoryExceptionHandling factory) : TimesheetComponentTestBase<ApiServiceWebApplicationFactoryExceptionHandling>(factory)
    {
        ~ExceptionHandlerComponentTests() => Dispose();

        [Scenario]
        public async Task AddEndpoint_ReturnsInternalServerError()
        {
            _ = Runner.RunScenarioAsync
                (
                    given => A_New_PersonId(),
                    when => An_Add_Request_Is_Performed(),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Scenario]
        public async Task PatchEndpoint_ReturnsInternalServerError()
        {
            _ = Runner.RunScenarioAsync
                (
                    given => A_New_PersonId(),
                    when => A_Patch_Request_Is_Performed(),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Scenario]
        public async Task GetEndpoint_ReturnsInternalServerError()
        {
            _ = Runner.RunScenarioAsync
                (
                    given => A_New_PersonId(),
                    when => A_Get_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Scenario]
        public async Task DeleteEndpoint_ReturnsInternalServerError()
        {
            _ = Runner.RunScenarioAsync
                (
                    given => A_New_PersonId(),
                    when => A_Delete_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Scenario]
        public async Task SearchEndpoint_ReturnsInternalServerError()
        {
            DateTime timeStamp = DateTime.UtcNow;

            _ = Runner.RunScenarioAsync
                (
                given => A_New_PersonId(),
                when => A_Search_Request_Is_Performed(timeStamp, timeStamp.AddDays(7)),
                then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }
    }
}
