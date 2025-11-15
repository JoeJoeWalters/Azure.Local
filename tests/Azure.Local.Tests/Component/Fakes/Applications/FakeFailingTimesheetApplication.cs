using Azure.Local.Application.Timesheets;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Tests.Component.Fakes.Applications
{
    public class FakeFailingTimesheetApplication : ITimesheetApplication
    {
        public Task<bool> AddAsync(TimesheetItem item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TimesheetItem?> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TimesheetItem item)
        {
            throw new NotImplementedException();
        }
    }
}
