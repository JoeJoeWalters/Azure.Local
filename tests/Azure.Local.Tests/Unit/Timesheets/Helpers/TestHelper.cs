using Azure.Local.Domain.Timesheets;
using System.Diagnostics.CodeAnalysis;

namespace Azure.Local.Tests.Unit.Timesheets.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class TestHelper
    {
        public static TimesheetItem CreateTestItem() => CreateTestItem(Guid.NewGuid().ToString());

        public static TimesheetItem CreateTestItem(string personId) 
            => new()
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = personId,
                From = DateTime.UtcNow.AddHours(-2),
                To = DateTime.UtcNow,
                Components =
                [
                    new() {
                        From = DateTime.UtcNow.AddHours(-2),
                        To = DateTime.UtcNow,
                        Units = 2,
                        TimeCode = Guid.NewGuid().ToString(),
                        ProjectCode = Guid.NewGuid().ToString()
                    }
                ]
            };
    }
}
