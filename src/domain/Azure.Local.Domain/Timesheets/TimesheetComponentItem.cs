namespace Azure.Local.Domain.Timesheets
{
    public class TimesheetComponentItem
    {
        // Identity
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Time & Duration
        public required double Units { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }

        // Coding
        public required string TimeCode { get; set; }
        public required string ProjectCode { get; set; }

        // Work Details
        public string? Description { get; set; }
        public WorkType WorkType { get; set; } = WorkType.Regular;

        // Classification
        public bool IsBillable { get; set; } = true;

        // Control
        public bool IsLocked { get; set; } = false;
    }
}
