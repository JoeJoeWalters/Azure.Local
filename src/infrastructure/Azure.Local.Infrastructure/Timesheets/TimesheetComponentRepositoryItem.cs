namespace Azure.Local.Infrastructure.Timesheets
{
    public class TimesheetComponentRepositoryItem
    {
        public TimesheetComponentRepositoryItem() { 
        
        }

        public double Units { get; set; } = 0.0D;
        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MinValue;
    }
}
