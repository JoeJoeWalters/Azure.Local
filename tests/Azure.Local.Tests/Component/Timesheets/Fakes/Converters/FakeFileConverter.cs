using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters;

namespace Azure.Local.Tests.Component.Timesheets.Fakes.Converters
{
    [ExcludeFromCodeCoverage]
    public class FakeFileConverter : IFileConverter
    {
        public Task<TimesheetItem?> ConvertAsync(string personId, Stream fileStream)
        {
            var timesheetItem = new TimesheetItem
            {
                PersonId = personId,
                From = DateTime.UtcNow,
                To = DateTime.UtcNow.AddDays(7),
                CreatedBy = personId,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = TimesheetStatus.Draft
            };

            return Task.FromResult<TimesheetItem?>(timesheetItem);
        }
    }
}
