using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.Domain.Timesheets;
using System.Text.Json;

namespace Azure.Local.ApiService.Timesheets.Helpers
{
    public static class CastHelper
    {
        extension(AddTimesheetHttpRequest request)
        {
            public TimesheetItem ToTimesheetItem()
            {
                return new TimesheetItem
                {
                    Id = request.Id,
                    PersonId = request.PersonId,
                    From = request.From,
                    To = request.To,
                    Components = [.. request.Components.Select(c => c.ToTimesheetComponentItem())]
                };
            }
        }

        extension(TimesheetHttpRequestComponent component)
        {
            public TimesheetComponentItem ToTimesheetComponentItem()
            {
                return new TimesheetComponentItem
                {
                    Units = component.Units,
                    From = component.From,
                    To = component.To,
                    TimeCode = component.TimeCode,
                    ProjectCode = component.ProjectCode
                };
            }
        }

        extension(AddTimesheetHttpRequest? item)
        {
            public PatchTimesheetHttpRequest? ToPatchTimesheetHttpRequest()
            {
                if (item is null)
                    return null;

                return new PatchTimesheetHttpRequest
                {
                    Id = item.Id,
                    PersonId = item.PersonId,
                    From = item.From,
                    To = item.To,
                    Components = [.. item.Components.Select(c => c.Clone())]
                };
            }
        }

        extension<T>(T source)
        {
            public T Clone()
            {
                var serialized = JsonSerializer.Serialize(source);
                var deserialized = JsonSerializer.Deserialize<T>(serialized);
                return deserialized is null
                    ? throw new InvalidOperationException($"Deserialization of type {typeof(T).FullName} resulted in null.")
                    : deserialized;
            }
        }
    }
}
