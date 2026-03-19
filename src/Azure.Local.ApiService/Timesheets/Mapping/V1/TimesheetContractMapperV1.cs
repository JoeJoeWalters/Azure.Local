using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.ApiService.Timesheets.Helpers;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Timesheets.Mapping.V1
{
    public class TimesheetContractMapperV1 : ITimesheetContractMapperV1
    {
        public TimesheetItem ToDomain(AddTimesheetHttpRequest request)
            => request.ToTimesheetItem();

        public TimesheetItem ToDomain(PatchTimesheetHttpRequest request)
            => request.ToTimesheetItem();

        public TimesheetResponse ToResponse(TimesheetItem item)
            => item.ToTimesheetResponse();

        public List<TimesheetResponse> ToResponse(IEnumerable<TimesheetItem> items)
            => items.Select(item => item.ToTimesheetResponse()).ToList();
    }
}
