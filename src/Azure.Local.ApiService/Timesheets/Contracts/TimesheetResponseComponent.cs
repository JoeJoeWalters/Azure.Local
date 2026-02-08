namespace Azure.Local.ApiService.Timesheets.Contracts
{
    /// <summary>
    /// Component (line item) in a timesheet response
    /// </summary>
    public class TimesheetResponseComponent
    {
        // Identity
        public required string Id { get; set; }

        // Time & Duration
        public required double Units { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }

        // Coding
        public required string TimeCode { get; set; }
        public required string ProjectCode { get; set; }

        // Work Details
        public string? Description { get; set; }
        public required string WorkType { get; set; }

        // Classification
        public bool IsBillable { get; set; }

        // Control
        public bool IsLocked { get; set; }
    }
}
