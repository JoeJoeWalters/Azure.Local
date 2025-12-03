using Azure.Local.Domain.Timesheets;
using System.Diagnostics.CodeAnalysis;

namespace Azure.Local.ApiService.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
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
