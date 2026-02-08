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
                    // Core
                    Id = item.Id,
                    PersonId = item.PersonId,
                    From = item.From,
                    To = item.To,
                    Components = [.. item.Components.Select(c => c.ToTimesheetComponentRepositoryItem())],

                    // Workflow
                    Status = item.Status,
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

                    // Concurrency
                    ETag = item.ETag,
                    Version = item.Version
                };
        }

        extension(TimesheetComponentItem item) 
        {
            public TimesheetComponentRepositoryItem ToTimesheetComponentRepositoryItem()
                => new()
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
                    WorkType = item.WorkType,

                    // Classification
                    IsBillable = item.IsBillable,

                    // Control
                    IsLocked = item.IsLocked
                };
        }

        extension(TimesheetRepositoryItem item)
        {
            public TimesheetItem ToTimesheetItem()
            => new()
            {
                // Core
                Id = item.Id,
                PersonId = item.PersonId,
                From = item.From,
                To = item.To,
                Components = [.. item.Components.Select(c => c.ToTimesheetComponentItem())],

                // Workflow
                Status = item.Status,
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

                // Concurrency
                ETag = item.ETag,
                Version = item.Version
            };
        }

        extension(TimesheetComponentRepositoryItem item)
        {
            public TimesheetComponentItem ToTimesheetComponentItem()
                => new()
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
                    WorkType = item.WorkType,

                    // Classification
                    IsBillable = item.IsBillable,

                    // Control
                    IsLocked = item.IsLocked
                };
        }
    }
}
