using System;

namespace Azure.Local.Infrastructure.Repository
{
	public class CosmosRepository<T> : IRepository<T> where T : class
    {
		public CosmosRepository()
		{

		}

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public T GetById(string id)
        {
            throw new NotImplementedException();
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
