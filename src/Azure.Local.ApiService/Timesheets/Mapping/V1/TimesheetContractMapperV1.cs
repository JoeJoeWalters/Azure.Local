using Azure.Local.ApiService.Timesheets.Contracts.V1;
using Azure.Local.ApiService.Timesheets.Helpers;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Timesheets.Mapping.V1
{
    public class TimesheetContractMapperV1 : ITimesheetContractMapperV1
    {
        public TimesheetItem ToDomain(AddTimesheetHttpRequestV1 request)
            => request.ToTimesheetItem();

        public TimesheetItem ToDomain(PatchTimesheetHttpRequestV1 request)
            => request.ToTimesheetItem();

        public TimesheetResponseV1 ToResponse(TimesheetItem item)
            => item.ToTimesheetResponse();

        public List<TimesheetResponseV1> ToResponse(IEnumerable<TimesheetItem> items)
            => items.Select(item => item.ToTimesheetResponse()).ToList();
    }
}
