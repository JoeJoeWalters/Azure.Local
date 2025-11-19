namespace Azure.Local.Domain.Timesheets
{
    public class TimesheetItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string PersonId { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
        public List<TimesheetComponentItem> Components { get; set; } = new List<TimesheetComponentItem>();
    }
}
