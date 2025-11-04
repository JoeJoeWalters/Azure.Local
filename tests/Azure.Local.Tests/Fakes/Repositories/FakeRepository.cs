using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Repository.Specifications;
using System;

namespace Azure.Local.ApiService.Tests.Component.Fakes.Repositories
{
	public class FakeRepository<T> : IRepository<T> where T : IRepositoryItem
    {
		private readonly Dictionary<string, T> _items;

        public FakeRepository()
		{
            _items = new Dictionary<string, T>();
        }

        public void Add(T item)
            => _items.Add(item.Id, item);

        public IEnumerable<T> Query(GenericSpecification<T> expression, int take = 0)
            => (take == 0 ?
                _items.Select(x => x.Value).AsQueryable().Where(expression.Expression) :
                _items.Select(x => x.Value).AsQueryable().Where(expression.Expression).Take(take)).ToList();

        public void Update(T item)
        {
            _items[item.Id] = item;
        }

        public void Upsert(T item)
        {
            if (_items.ContainsKey(item.Id))
                Update(item);
            else
                Add(item);
        }
    }
}
