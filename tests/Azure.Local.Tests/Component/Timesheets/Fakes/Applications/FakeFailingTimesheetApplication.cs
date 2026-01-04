using Azure.Local.Application.Timesheets;
using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Timesheets.FileProcessing;

namespace Azure.Local.Tests.Component.Timesheets.Fakes.Applications
{
    [ExcludeFromCodeCoverage]
    public class FakeFailingTimesheetApplication : ITimesheetApplication
    {
        public Task<bool> AddAsync(string personId, TimesheetItem item) => throw new NotImplementedException();
        public Task<bool> DeleteAsync(string personId, string id) => throw new NotImplementedException();
        public Task<TimesheetItem?> GetAsync(string personId, string id) => throw new NotImplementedException();
        public Task<List<TimesheetItem>> SearchAsync(string personId, DateTime fromDate, DateTime toDate) => throw new NotImplementedException();
        public Task<bool> UpdateAsync(string personId, TimesheetItem item) => throw new NotImplementedException();
        public Task<TimesheetItem?> ProcessFileAsync(string personId, Stream stream, TimesheetFileTypes fileType) => throw new NotImplementedException();
    }
}
