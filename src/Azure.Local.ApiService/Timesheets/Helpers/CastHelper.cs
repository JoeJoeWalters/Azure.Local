using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Test.Helpers
{
    public static class CastHelper
    {
        public static TimesheetItem ToTimesheetItem(this AddTimesheetHttpRequest request)
        {
            return new TimesheetItem
            {
                Id = request.Id,
                PersonId = request.PersonId,
                From = request.From,
                To = request.To,
                Components = request.Components
                                .Select(c => c.ToTimesheetComponentItem())
                                .ToList()
            };
        }

        public static TimesheetComponentItem ToTimesheetComponentItem(this AddTimesheetHttpRequestComponent component)
        {
            return new TimesheetComponentItem
            {
                Units = component.Units,
                From = component.From,
                To = component.To,
                Code = component.Code
            };
        }
    }
}
