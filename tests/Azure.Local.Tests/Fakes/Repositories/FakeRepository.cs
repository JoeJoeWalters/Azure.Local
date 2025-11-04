using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Repository.Specifications;
using System;
using System.Collections.Generic;

namespace Azure.Local.ApiService.Tests.Component.Fakes.Repositories
{
	public class FakeRepository<T> : IRepository<T> where T : IRepositoryItem
    {
		private readonly Dictionary<string, T> _items;

        public FakeRepository()
		{
            _items = new Dictionary<string, T>();
        }

        public async void Add(T item)
            => _items.Add(item.Id, item);

        public async Task<IEnumerable<T>> Query(GenericSpecification<T> expression, int take = 0)
            => (take == 0 ?
                _items.Select(x => x.Value).AsQueryable().Where(expression.Expression) :
                _items.Select(x => x.Value).AsQueryable().Where(expression.Expression).Take(take)).ToList();

        public async void Update(T item)
        {
            _items[item.Id] = item;
        }

        public async void Upsert(T item)
        {
            if (_items.ContainsKey(item.Id))
                Update(item);
            else
                Add(item);
        }
    }
}
