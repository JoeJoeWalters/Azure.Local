using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Timesheets;

namespace Azure.Local.Application.Timesheets.Helpers
{
    public static class CastHelper
    {
        public static TimesheetRepositoryItem ToTimesheetRepositoryItem(this TimesheetItem item)
            => new TimesheetRepositoryItem
            {
                Id = item.Id,
                PersonId = item.PersonId,
                From = item.From,
                To = item.To,
                Components = item.Components.Select(c => c.ToTimesheetComponentRepositoryItem()).ToList()
            };

        public static TimesheetComponentRepositoryItem ToTimesheetComponentRepositoryItem(this TimesheetComponentItem item)
            => new TimesheetComponentRepositoryItem
            {
                Units = item.Units,
                From = item.From,
                To = item.To,
                Code = item.Code
            };

        public static TimesheetItem ToTimesheetItem(this TimesheetRepositoryItem item)
            => new TimesheetItem
            {
                Id = item.Id,
                PersonId = item.PersonId,
                From = item.From,
                To = item.To,
                Components = item.Components.Select(c => c.ToTimesheetComponentItem()).ToList()
            };

        public static TimesheetComponentItem ToTimesheetComponentItem(this TimesheetComponentRepositoryItem item)
            => new TimesheetComponentItem
            {
                Units = item.Units,
                From = item.From,
                To = item.To,
                Code = item.Code
            };
    }
}
