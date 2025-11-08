namespace Azure.Local.Domain.Timesheets
{
    public class TimesheetItem
    {
        public string Id { get; set; } = string.Empty;
        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MinValue;
        public List<TimesheetComponentItem> Components { get; set; } = new List<TimesheetComponentItem>();
    }
}
