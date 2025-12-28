using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Timesheets;

namespace Azure.Local.Application.Timesheets.Helpers
{
    public static class CastHelper
    {
        public static TimesheetRepositoryItem ToTimesheetRepositoryItem(this TimesheetItem item)
            => new()
            {
                Id = item.Id,
                PersonId = item.PersonId,
                From = item.From,
                To = item.To,
                Components = [.. item.Components.Select(c => c.ToTimesheetComponentRepositoryItem())]
            };

        public static TimesheetComponentRepositoryItem ToTimesheetComponentRepositoryItem(this TimesheetComponentItem item)
            => new()
            {
                Units = item.Units,
                From = item.From,
                To = item.To,
                Code = item.Code
            };

        public static TimesheetItem ToTimesheetItem(this TimesheetRepositoryItem item)
            => new()
            {
                Id = item.Id,
                PersonId = item.PersonId,
                From = item.From,
                To = item.To,
                Components = [.. item.Components.Select(c => c.ToTimesheetComponentItem())]
            };

        public static TimesheetComponentItem ToTimesheetComponentItem(this TimesheetComponentRepositoryItem item)
            => new()
            {
                Units = item.Units,
                From = item.From,
                To = item.To,
                Code = item.Code
            };
    }
}
