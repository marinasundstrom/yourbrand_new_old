using System.Linq.Expressions;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.Specifications;

namespace YourBrand.YourService.API.Features.Todos;

public class HasExpired(TimeSpan expirationTime) : Specification<Todo>
{
    public override Expression<Func<Todo, bool>> ToExpression()
        => order => DateTimeOffset.UtcNow - order.Created > expirationTime;
}