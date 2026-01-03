using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Timesheets.FileProcessing;

namespace Azure.Local.Infrastructure.Timesheets
{
    public class TimesheetRepositoryItem : RepositoryItem
    {
        public TimesheetRepositoryItem() { 
            Id = Guid.NewGuid().ToString();
        }
        public required string PersonId { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; } 
        public List<TimesheetComponentRepositoryItem> Components { get; set; } = [];
        public TimesheetFileTypes FileType { get; set; } = TimesheetFileTypes.None;
    }
}
