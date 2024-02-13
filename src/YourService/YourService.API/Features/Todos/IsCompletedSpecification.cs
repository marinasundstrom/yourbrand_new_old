using System.Linq.Expressions;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.Specifications;

namespace YourBrand.YourService.API.Features.Todos;

public class IsCompletedSpecification(bool isCompleted = true) : Specification<Todo>
{
    public override Expression<Func<Todo, bool>> ToExpression()
        => todo => todo.IsCompleted == isCompleted;
}