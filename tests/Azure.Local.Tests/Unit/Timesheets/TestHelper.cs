using Azure.Local.Domain.Timesheets;
using System.Diagnostics.CodeAnalysis;
using System.CodeDom.Compiler;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [GeneratedCodeAttribute("System.Text.RegularExpressions.Generator", "8.0.8.47906")]

    [ExcludeFromCodeCoverage]
    public static class TestHelper
    {
        public static TimesheetItem CreateTestItem() => CreateTestItem(Guid.NewGuid().ToString());

        public static TimesheetItem CreateTestItem(string personId) 
            => new TimesheetItem
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = personId,
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
