namespace Azure.Local.Domain.Timesheets
{
    public class TimesheetItem
    {
        // Core Identity
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string PersonId { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
        public List<TimesheetComponentItem> Components { get; set; } = [];

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

        // Computed Properties
        public double TotalUnits => Components.Sum(c => c.Units);

        // Helper Methods
        public bool CanEdit() => Status == TimesheetStatus.Draft || Status == TimesheetStatus.Recalled;
        public bool CanSubmit() => Status == TimesheetStatus.Draft && Components.Count > 0;
        public bool CanApprove() => Status == TimesheetStatus.Submitted;
        public bool CanReject() => Status == TimesheetStatus.Submitted;
        public bool CanRecall() => Status == TimesheetStatus.Submitted;
    }
}
