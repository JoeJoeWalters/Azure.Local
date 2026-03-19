using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Timesheets.Mapping.V1
{
    public interface ITimesheetContractMapperV1
    {
        TimesheetItem ToDomain(AddTimesheetHttpRequest request);
        TimesheetItem ToDomain(PatchTimesheetHttpRequest request);
        TimesheetResponse ToResponse(TimesheetItem item);
        List<TimesheetResponse> ToResponse(IEnumerable<TimesheetItem> items);
    }
}
