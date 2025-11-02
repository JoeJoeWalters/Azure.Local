using System.Linq.Expressions;

namespace Azure.Local.Infrastructure.Repository.Specifications
{
    public abstract class GenericSpecification<T>
    {
        public Expression<Func<T, bool>> Expression { get; protected set; }
    }
}
