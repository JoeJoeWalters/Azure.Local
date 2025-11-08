using Azure.Local.Infrastructure.Repository;

namespace Azure.Local.Infrastructure.Timesheets
{
    public class TimesheetRepositoryItem : RepositoryItem
    {
        public TimesheetRepositoryItem() { 
            Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; } = string.Empty;
    }
}
