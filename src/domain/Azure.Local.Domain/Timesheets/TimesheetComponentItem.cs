namespace Azure.Local.Domain.Timesheets
{
    public class TimesheetComponentItem
    {
        public required double Units { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
        public required string Code { get; set; }
    }
}
