using System.Linq.Expressions;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.Specifications;

namespace YourBrand.YourService.API.Features.Todos;

public class IsCompleted(bool isCompleted = true) : Specification<Todo>
{
    public override Expression<Func<Todo, bool>> ToExpression()
        => order => order.IsCompleted == isCompleted;
}
