using Azure.Local.Infrastructure.Repository.Specifications;

namespace Azure.Local.Infrastructure.Repository
{
    /// <summary>
    /// Repository pattern where there is no compound / composite key
    /// </summary>
    public interface IRepository<T> where T : RepositoryItem
    {
        public Task<bool> AddAsync(T item);
        public Task<bool> UpdateAsync(T item);
        public Task<bool> UpsertAsync(T item);
        public Task<IEnumerable<T>> QueryAsync(GenericSpecification<T> expression, int take = 0);
        public Task<bool> DeleteAsync(GenericSpecification<T> expression);
    }
}