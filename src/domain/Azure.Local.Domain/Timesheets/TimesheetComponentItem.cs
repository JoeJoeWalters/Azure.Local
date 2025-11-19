namespace Azure.Local.Domain.Timesheets
{
    public class TimesheetComponentItem
    {
        public double Units { get; set; } = 0.0D;
        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MinValue;
        public string Code { get; set; } = string.Empty;
    }
}
