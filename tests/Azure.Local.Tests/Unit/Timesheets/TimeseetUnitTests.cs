using Azure.Local.Infrastructure.Timesheets;

namespace Azure.Local.ApiService.Tests.Unit.Timesheets
{
    public class TimeseetUnitTests
    {
        public TimesheetRepositoryItem CreateTestItem() 
            => new TimesheetRepositoryItem
            {
                Id = Guid.NewGuid().ToString(),
                From = DateTime.UtcNow.AddHours(-2),
                To = DateTime.UtcNow,
                Components = new List<TimesheetComponentRepositoryItem>
                {
                    new TimesheetComponentRepositoryItem
                    {
                        From = DateTime.UtcNow.AddHours(-2),
                        To = DateTime.UtcNow,
                        Units = 2
                    }
                }
            };
    }
}
