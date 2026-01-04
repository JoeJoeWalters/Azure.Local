using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters;

namespace Azure.Local.Tests.Component.Timesheets.Fakes.Converters
{
    [ExcludeFromCodeCoverage]
    public class FakeFileConverter : IFileConverter
    {
        public Task<TimesheetItem?> ConvertAsync(Stream fileStream)
        {
            var timesheetItem = new TimesheetItem
            {
                PersonId = Guid.NewGuid().ToString(),
                From = DateTime.UtcNow,
                To = DateTime.UtcNow.AddDays(7)
            };

            return Task.FromResult<TimesheetItem?>(timesheetItem);
        }
    }
}
