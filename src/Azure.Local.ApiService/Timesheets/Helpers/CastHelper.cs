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
                    // Core
                    Id = request.Id,
                    PersonId = request.PersonId,
                    From = request.From,
                    To = request.To,
                    Components = [.. request.Components.Select(c => c.ToTimesheetComponentItem())],

                    // Workflow (API can only create drafts)
                    Status = TimesheetStatus.Draft,

                    // Audit
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    CreatedBy = request.CreatedBy,

                    // Metadata
                    ManagerId = request.ManagerId,
                    Comments = request.Comments
                };
            }
        }

        extension(TimesheetHttpRequestComponent component)
        {
            public TimesheetComponentItem ToTimesheetComponentItem()
            {
                return new TimesheetComponentItem
                {
                    // Identity
                    Id = component.Id ?? Guid.NewGuid().ToString(),

                    // Time
                    Units = component.Units,
                    From = component.From,
                    To = component.To,

                    // Coding
                    TimeCode = component.TimeCode,
                    ProjectCode = component.ProjectCode,

                    // Work Details
                    Description = component.Description,
                    WorkType = ParseWorkType(component.WorkType),

                    // Classification
                    IsBillable = component.IsBillable ?? true
                };
            }
        }

        extension(TimesheetItem item)
        {
            public TimesheetResponse ToTimesheetResponse()
            {
                return new TimesheetResponse
                {
                    // Core
                    Id = item.Id,
                    PersonId = item.PersonId,
                    From = item.From,
                    To = item.To,
                    Components = [.. item.Components.Select(c => c.ToTimesheetResponseComponent())],

                    // Workflow
                    Status = item.Status.ToString(),
                    SubmittedDate = item.SubmittedDate,
                    ApprovedDate = item.ApprovedDate,
                    ApprovedBy = item.ApprovedBy,
                    RejectedDate = item.RejectedDate,
                    RejectedBy = item.RejectedBy,
                    RejectionReason = item.RejectionReason,

                    // Audit
                    CreatedDate = item.CreatedDate,
                    ModifiedDate = item.ModifiedDate,
                    CreatedBy = item.CreatedBy,
                    ModifiedBy = item.ModifiedBy,

                    // Metadata
                    ManagerId = item.ManagerId,
                    Comments = item.Comments,

                    // Computed
                    TotalUnits = item.TotalUnits,

                    // Concurrency
                    ETag = item.ETag,
                    Version = item.Version,

                    // Capabilities
                    CanEdit = item.CanEdit(),
                    CanSubmit = item.CanSubmit(),
                    CanApprove = item.CanApprove(),
                    CanReject = item.CanReject(),
                    CanRecall = item.CanRecall()
                };
            }
        }

        extension(TimesheetComponentItem item)
        {
            public TimesheetResponseComponent ToTimesheetResponseComponent()
            {
                return new TimesheetResponseComponent
                {
                    // Identity
                    Id = item.Id,

                    // Time
                    Units = item.Units,
                    From = item.From,
                    To = item.To,

                    // Coding
                    TimeCode = item.TimeCode,
                    ProjectCode = item.ProjectCode,

                    // Work Details
                    Description = item.Description,
                    WorkType = item.WorkType.ToString(),

                    // Classification
                    IsBillable = item.IsBillable,

                    // Control
                    IsLocked = item.IsLocked
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
                    Components = [.. item.Components.Select(c => c.Clone())],
                    ManagerId = item.ManagerId,
                    Comments = item.Comments,
                    CreatedBy = item.CreatedBy
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

        private static WorkType ParseWorkType(string? workType)
        {
            if (string.IsNullOrWhiteSpace(workType))
                return WorkType.Regular;

            if (Enum.TryParse<WorkType>(workType, true, out var result))
                return result;

            return WorkType.Regular;
        }
    }
}
