using Azure.Local.Infrastructure.Repository.Specifications;
using System;

namespace Azure.Local.Infrastructure.Repository
{
    public interface IRepository<T>
    {
        public void Add(T item);
        public void Update(T item);
        public void Upsert(T item);
        public IEnumerable<T> Query(GenericSpecification<T> expression, int take = 0);
    }
}