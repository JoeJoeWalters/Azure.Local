using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.Domain.Timesheets;
using System.Text.Json;

namespace Azure.Local.ApiService.Timesheets.Helpers
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
                Components = [.. request.Components.Select(c => c.ToTimesheetComponentItem())]
            };
        }

        public static TimesheetComponentItem ToTimesheetComponentItem(this TimesheetHttpRequestComponent component)
        {
            return new TimesheetComponentItem
            {
                Units = component.Units,
                From = component.From,
                To = component.To,
                Code = component.Code
            };
        }

        public static PatchTimesheetHttpRequest? ToPatchTimesheetHttpRequest(this AddTimesheetHttpRequest? item)
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
        public static T Clone<T>(this T source)
        {
            var serialized = JsonSerializer.Serialize(source);
            var deserialized = JsonSerializer.Deserialize<T>(serialized);
            return deserialized is null
                ? throw new InvalidOperationException($"Deserialization of type {typeof(T).FullName} resulted in null.")
                : deserialized;
        }
    }
}
