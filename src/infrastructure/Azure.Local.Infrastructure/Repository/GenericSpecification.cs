using System.Linq.Expressions;

namespace Azure.Local.Infrastructure.Repository
{
    public abstract class GenericSpecification<T>
    {
        public Expression<Func<T, Boolean>> Expression { get; protected set; }
    }
}
