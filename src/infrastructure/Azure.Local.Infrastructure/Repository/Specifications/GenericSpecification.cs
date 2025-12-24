using System.Linq.Expressions;

namespace Azure.Local.Infrastructure.Repository.Specifications
{
    public abstract class GenericSpecification<T>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Expression<Func<T, bool>> Expression { get; protected set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
