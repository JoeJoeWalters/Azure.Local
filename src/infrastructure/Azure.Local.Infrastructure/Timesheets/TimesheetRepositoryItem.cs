using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Timesheets.FileProcessing;

namespace Azure.Local.Infrastructure.Timesheets
{
    public class TimesheetRepositoryItem : RepositoryItem
    {
        public TimesheetRepositoryItem()
        {
            Id = Guid.NewGuid().ToString();
        }

        // Core
        public required string PersonId { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
        public List<TimesheetComponentRepositoryItem> Components { get; set; } = [];
        public TimesheetFileTypes FileType { get; set; } = TimesheetFileTypes.None;

        // Workflow & Status
        public TimesheetStatus Status { get; set; } = TimesheetStatus.Draft;
        public DateTime? SubmittedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? RejectedDate { get; set; }
        public string? RejectedBy { get; set; }
        public string? RejectionReason { get; set; }

        // Audit Trail
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public required string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        // Metadata
        public string? ManagerId { get; set; }
        public string? Comments { get; set; }

        // Concurrency Control
        public string? ETag { get; set; }
        public int Version { get; set; } = 1;
    }
}
