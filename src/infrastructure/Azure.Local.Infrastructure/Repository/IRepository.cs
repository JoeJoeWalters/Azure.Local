using Azure.Local.Infrastructure.Repository.Specifications;
using System;

namespace Azure.Local.Infrastructure.Repository
{
    /// <summary>
    /// Repository pattern where there is no compound / composite key
    /// </summary>
    public interface IRepository<T> where T : IRepositoryItem
    {
        public void Add(T item);
        public void Update(T item);
        public void Upsert(T item);
        public IEnumerable<T> Query(GenericSpecification<T> expression, int take = 0);
    }
}