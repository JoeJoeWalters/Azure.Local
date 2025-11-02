using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Repository.Specifications;
using System;

namespace Azure.Local.ApiService.Tests.Component.Fakes.Repositories
{
	public class FakeRepository<T> : IRepository<T> where T : class
    {
		private readonly HashSet<T> _items;

        public FakeRepository()
		{
            _items = new HashSet<T>();
        }

        public void Add(T item)
            => _items.Add(item);

        public IEnumerable<T> Query(GenericSpecification<T> expression, int take = 0)
            => (take == 0 ?
                _items.AsQueryable().Where(expression.Expression) :
                _items.AsQueryable().Where(expression.Expression).Take(take)).ToList();

        public void Update(T item)
        {
            throw new NotImplementedException();
        }

        public void Upsert(T item)
        {
            throw new NotImplementedException();
        }
    }
}
