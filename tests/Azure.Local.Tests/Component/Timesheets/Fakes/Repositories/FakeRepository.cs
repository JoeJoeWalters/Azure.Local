using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Repository.Specifications;

namespace Azure.Local.ApiService.Tests.Component.Timesheets.Fakes.Repositories
{
	public class FakeRepository<T> : IRepository<T> where T : RepositoryItem
    {
		private readonly Dictionary<string, T> _items;

        public FakeRepository()
		{
            _items = new Dictionary<string, T>();
        }

        public async Task<bool> AddAsync(T item)
        {
            try
            {
                _items.Add(item.Id, item);
                return true;
            }
            catch
            {
            }

            await Task.Yield(); // Ensures the method is truly asynchronous
            return false;
        }

        public async Task<IEnumerable<T>> QueryAsync(GenericSpecification<T> expression, int take = 0)
        {
            var result = (take == 0 ?
                _items.Select(x => x.Value).AsQueryable().Where(expression.Expression) :
                _items.Select(x => x.Value).AsQueryable().Where(expression.Expression).Take(take)).ToList();

            await Task.Yield(); // Ensures the method is truly asynchronous
            return result;
        }

        public async Task<bool> UpdateAsync(T item)
        {
            try
            {
                if (!_items.ContainsKey(item.Id))
                    return false;

                _items[item.Id] = item;
                await Task.Yield(); // Ensures the method is truly asynchronous
                return true;
            }
            catch
            {
            }

            return false;
        }

        public async Task<bool> UpsertAsync(T item)
        {
            if (_items.ContainsKey(item.Id))
                return await UpdateAsync(item);
            else
                return await AddAsync(item);
        }

        public Task<bool> DeleteAsync(GenericSpecification<T> expression)
        {
            var result = QueryAsync(expression).GetAwaiter().GetResult();
            if (result.Any())
            {
                bool success = true;
                foreach (var item in result)
                {
                    var deleteResult = _items.Remove(item.Id);

                    // If one fails they all fail
                    if (!deleteResult)
                        success = false;
                }
                return Task.FromResult(success);
            }
            else
            {
                return Task.FromResult(false);
            }
        }
    }
}
