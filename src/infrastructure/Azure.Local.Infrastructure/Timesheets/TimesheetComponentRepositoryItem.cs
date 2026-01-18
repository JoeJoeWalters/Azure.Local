namespace Azure.Local.Infrastructure.Timesheets
{
    public class TimesheetComponentRepositoryItem
    {
        public TimesheetComponentRepositoryItem() { 
        
        }

        public required double Units { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
        public required string TimeCode { get; set; }
        public required string ProjectCode { get; set; }
    }
}
