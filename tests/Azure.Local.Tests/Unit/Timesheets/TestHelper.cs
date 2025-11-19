using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Tests.Unit.Timesheets
{
    public static class TestHelper
    {
        public static TimesheetItem CreateTestItem() 
            => new TimesheetItem
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = Guid.NewGuid().ToString(),
                From = DateTime.UtcNow.AddHours(-2),
                To = DateTime.UtcNow,
                Components = new List<TimesheetComponentItem>
                {
                    new TimesheetComponentItem
                    {
                        From = DateTime.UtcNow.AddHours(-2),
                        To = DateTime.UtcNow,
                        Units = 2,
                        Code = Guid.NewGuid().ToString()
                    }
                }
            };
    }
}
