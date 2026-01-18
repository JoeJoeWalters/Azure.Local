using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Timesheets;

namespace Azure.Local.Application.Timesheets.Helpers
{
    public static class CastHelper
    {
        extension(TimesheetItem item) 
        {
            public TimesheetRepositoryItem ToTimesheetRepositoryItem()
                => new()
                {
                    Id = item.Id,
                    PersonId = item.PersonId,
                    From = item.From,
                    To = item.To,
                    Components = [.. item.Components.Select(c => c.ToTimesheetComponentRepositoryItem())]
                };
        }

        extension(TimesheetComponentItem item) 
        {
            public TimesheetComponentRepositoryItem ToTimesheetComponentRepositoryItem()
                => new()
                {
                    Units = item.Units,
                    From = item.From,
                    To = item.To,
                    TimeCode = item.TimeCode
                };
        }

        extension(TimesheetRepositoryItem item)
        {
            public TimesheetItem ToTimesheetItem()
            => new()
            {
                Id = item.Id,
                PersonId = item.PersonId,
                From = item.From,
                To = item.To,
                Components = [.. item.Components.Select(c => c.ToTimesheetComponentItem())]
            };
        }

        extension(TimesheetComponentRepositoryItem item)
        {
            public TimesheetComponentItem ToTimesheetComponentItem()
                => new()
                {
                    Units = item.Units,
                    From = item.From,
                    To = item.To,
                    TimeCode = item.TimeCode
                };
        }
    }
}
