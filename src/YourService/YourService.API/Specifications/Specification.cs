using System.Linq;
using System.Linq.Expressions;

using LinqKit;

namespace YourBrand.YourService.API.Domain.Specifications;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        Func<T, bool> predicate = ToExpression().Compile();
        return predicate(entity);
    }
}

public class AndSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression() => left.ToExpression().And(right.ToExpression()).Expand();
}

public class OrSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression() => left.ToExpression().Or(right.ToExpression()).Expand();
}

public static class SpecificationExtensions
{
    public static Specification<T> And<T>(this Specification<T> left, Specification<T> right) => new AndSpecification<T>(left, right);

    public static Specification<T> Or<T>(this Specification<T> left, Specification<T> right) => new OrSpecification<T>(left, right);
}