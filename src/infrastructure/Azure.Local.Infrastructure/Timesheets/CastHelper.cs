using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Infrastructure.Timesheets
{
    internal static class CastHelper
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
                    Components = item.Components.Select(c => c.ToTimesheetComponentRepositoryItem()).ToList(),

                    Status = item.Status,
                    SubmittedDate = item.SubmittedDate,
                    ApprovedDate = item.ApprovedDate,
                    ApprovedBy = item.ApprovedBy,
                    RejectedDate = item.RejectedDate,
                    RejectedBy = item.RejectedBy,
                    RejectionReason = item.RejectionReason,

                    CreatedDate = item.CreatedDate,
                    ModifiedDate = item.ModifiedDate,
                    CreatedBy = item.CreatedBy,
                    ModifiedBy = item.ModifiedBy,

                    ManagerId = item.ManagerId,
                    Comments = item.Comments,

                    ETag = item.ETag,
                    Version = item.Version
                };
        }

        extension(TimesheetComponentItem item)
        {
            public TimesheetComponentRepositoryItem ToTimesheetComponentRepositoryItem()
                => new()
                {
                    Id = item.Id,
                    Units = item.Units,
                    From = item.From,
                    To = item.To,
                    TimeCode = item.TimeCode,
                    ProjectCode = item.ProjectCode,
                    Description = item.Description,
                    WorkType = item.WorkType,
                    IsBillable = item.IsBillable,
                    IsLocked = item.IsLocked
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
                    Components = item.Components.Select(c => c.ToTimesheetComponentItem()).ToList(),

                    Status = item.Status,
                    SubmittedDate = item.SubmittedDate,
                    ApprovedDate = item.ApprovedDate,
                    ApprovedBy = item.ApprovedBy,
                    RejectedDate = item.RejectedDate,
                    RejectedBy = item.RejectedBy,
                    RejectionReason = item.RejectionReason,

                    CreatedDate = item.CreatedDate,
                    ModifiedDate = item.ModifiedDate,
                    CreatedBy = item.CreatedBy,
                    ModifiedBy = item.ModifiedBy,

                    ManagerId = item.ManagerId,
                    Comments = item.Comments,

                    ETag = item.ETag,
                    Version = item.Version
                };
        }

        extension(TimesheetComponentRepositoryItem item)
        {
            public TimesheetComponentItem ToTimesheetComponentItem()
                => new()
                {
                    Id = item.Id,
                    Units = item.Units,
                    From = item.From,
                    To = item.To,
                    TimeCode = item.TimeCode,
                    ProjectCode = item.ProjectCode,
                    Description = item.Description,
                    WorkType = item.WorkType,
                    IsBillable = item.IsBillable,
                    IsLocked = item.IsLocked
                };
        }
    }
}
