using System.Linq.Expressions;

namespace EficazAPI.Domain.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
        bool IsSatisfiedBy(T entity);
    }
}
