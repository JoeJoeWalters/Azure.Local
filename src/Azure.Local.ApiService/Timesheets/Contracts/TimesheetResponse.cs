namespace Azure.Local.ApiService.Timesheets.Contracts
{
    /// <summary>
    /// Timesheet response with full details for GET operations
    /// </summary>
    public class TimesheetResponse
    {
        // Core
        public required string Id { get; set; }
        public required string PersonId { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
        public List<TimesheetResponseComponent> Components { get; set; } = [];

        // Workflow
        public required string Status { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? RejectedDate { get; set; }
        public string? RejectedBy { get; set; }
        public string? RejectionReason { get; set; }

        // Audit
        public required DateTime CreatedDate { get; set; }
        public required DateTime ModifiedDate { get; set; }
        public required string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        // Metadata
        public string? ManagerId { get; set; }
        public string? Comments { get; set; }

        // Computed Totals
        public double TotalUnits { get; set; }

        // Concurrency
        public string? ETag { get; set; }
        public int Version { get; set; }

        // Capabilities
        public bool CanEdit { get; set; }
        public bool CanSubmit { get; set; }
        public bool CanApprove { get; set; }
        public bool CanReject { get; set; }
        public bool CanRecall { get; set; }
    }
}
