using Azure.Local.Infrastructure.Repository;

namespace Azure.Local.Infrastructure.Timesheets
{
    public class TimesheetRepositoryItem : RepositoryItem
    {
        public TimesheetRepositoryItem() { 
            Id = Guid.NewGuid().ToString();
        }

        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MinValue; 
        public List<TimesheetComponentRepositoryItem> Components { get; set; } = new List<TimesheetComponentRepositoryItem>();
    }
}
