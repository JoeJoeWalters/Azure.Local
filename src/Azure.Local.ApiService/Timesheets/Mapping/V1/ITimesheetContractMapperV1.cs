using Azure.Local.ApiService.Timesheets.Contracts.V1;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Timesheets.Mapping.V1
{
    public interface ITimesheetContractMapperV1
    {
        TimesheetItem ToDomain(AddTimesheetHttpRequestV1 request);
        TimesheetItem ToDomain(PatchTimesheetHttpRequestV1 request);
        TimesheetResponseV1 ToResponse(TimesheetItem item);
        List<TimesheetResponseV1> ToResponse(IEnumerable<TimesheetItem> items);
    }
}
