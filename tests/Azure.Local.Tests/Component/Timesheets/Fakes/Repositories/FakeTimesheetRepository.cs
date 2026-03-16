using Azure.Local.Application.Timesheets;
using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository.Specifications;
using Azure.Local.Infrastructure.Repository.Specifications.Timesheets;

namespace Azure.Local.Tests.Component.Timesheets.Fakes.Repositories
{
    [ExcludeFromCodeCoverage]
    public class FakeTimesheetRepository : ITimesheetRepository
    {
        private readonly Dictionary<string, TimesheetItem> _items = [];

        public Task<bool> AddAsync(TimesheetItem item)
        {
            if (_items.ContainsKey(item.Id))
                return Task.FromResult(false);
            _items[item.Id] = item;
            return Task.FromResult(true);
        }

        public Task<bool> UpdateAsync(TimesheetItem item)
        {
            if (!_items.ContainsKey(item.Id))
                return Task.FromResult(false);
            _items[item.Id] = item;
            return Task.FromResult(true);
        }

        public Task<TimesheetItem?> GetByIdAsync(string id)
        {
            _items.TryGetValue(id, out var item);
            return Task.FromResult(item);
        }

        public Task<bool> DeleteByIdAsync(string id)
        {
            var removed = _items.Remove(id);
            return Task.FromResult(removed);
        }

        public Task<IEnumerable<TimesheetItem>> SearchAsync(string personId, DateTime fromDate, DateTime toDate)
        {
            var results = _items.Values
                .Where(i => i.PersonId == personId && i.From <= toDate && i.To >= fromDate);
            return Task.FromResult(results);
        }
    }
}
