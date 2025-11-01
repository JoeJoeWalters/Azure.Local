using System;

namespace Azure.Local.Infrastructure.Repository
{
    public interface IRepository<T>
    {
        public void Add(T item);
        public void Update(T item);
        public void Upsert(T item);
        public T GetById(string id);
        public IEnumerable<T> Query();
    }
}