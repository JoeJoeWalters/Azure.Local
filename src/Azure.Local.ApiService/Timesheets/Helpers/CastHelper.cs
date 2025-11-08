using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.Domain.Test;

namespace Azure.Local.ApiService.Test.Helpers
{
    public static class CastHelper
    {
        public static TimesheetItem ToTimesheetItem(this AddTimesheetHttpRequest request)
        {
            return new TimesheetItem
            {
                Id = request.Id,
                Name = request.Name
            };
        }
    }
}
