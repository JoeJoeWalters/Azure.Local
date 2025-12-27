using Azure.Local.ApiService.Tests.Component.Timesheets.Setup;
using Azure.Local.Tests.Component.Setup;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace Azure.Local.Tests.Component.Timesheets
{
    [Collection("NoParallelization")]
    public class ExceptionHandlerComponentTests(ApiServiceWebApplicationFactoryExceptionHandling factory) : ComponentTestBase<ApiServiceWebApplicationFactoryExceptionHandling>(factory)
    {
        private const string _endpoint = "/person/{personId}/timesheet/item";
        private const string _searchEndpoint = "/person/{personId}/timesheet/search";

        ~ExceptionHandlerComponentTests() => Dispose();

        [Scenario]
        public void AddEndpoint_ReturnsInternalServerError()
        {
            Runner.RunScenario
                (
                    given => A_New_PersonId(),
                    when => An_Add_Request_Is_Performed(),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Scenario]
        public void PatchEndpoint_ReturnsInternalServerError()
        {
            Runner.RunScenario
                (
                    given => A_New_PersonId(),
                    when => A_Patch_Request_Is_Performed(),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Scenario]
        public void GetEndpoint_ReturnsInternalServerError()
        {
            Runner.RunScenario
                (
                    given => A_New_PersonId(),
                    when => A_Get_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Scenario]
        public void DeleteEndpoint_ReturnsInternalServerError()
        {
            Runner.RunScenario
                (
                    given => A_New_PersonId(),
                    when => A_Delete_Request_Is_Performed(Guid.NewGuid().ToString()),
                    then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }

        [Scenario]
        public void SearchEndpoint_ReturnsInternalServerError()
        {
            DateTime timeStamp = DateTime.UtcNow;

            Runner.RunScenario
                (
                given => A_New_PersonId(),
                when => A_Search_Request_Is_Performed(timeStamp, timeStamp.AddDays(7)),
                then => The_Response_Should_Be(HttpStatusCode.InternalServerError)
                );
        }
    }
}
