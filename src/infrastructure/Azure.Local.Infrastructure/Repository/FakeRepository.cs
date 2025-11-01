using System;

namespace Azure.Local.Infrastructure.Repository
{
	public class FakeRepository<T> : IRepository<T> where T : IRepositoryItem
    {
		private readonly Dictionary<string, T> _items;

        public FakeRepository()
		{
            _items = new Dictionary<string, T>();
        }

        public void Add(T item)
        {
            _items.Add(item.Id, item);
        }

        public T GetById(string id)
        {
            _items.TryGetValue(id, out T item);
            return item;
        }

        public IEnumerable<T> Query()
        {
            throw new NotImplementedException();
        }

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
