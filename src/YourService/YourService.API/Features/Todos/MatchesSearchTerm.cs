using System.Linq.Expressions;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.Specifications;

namespace YourBrand.YourService.API.Features.Todos;

public class MatchesSearchTerm(string searchTerm) : Specification<Todo>
{
    public override Expression<Func<Todo, bool>> ToExpression()
        => todo => todo.Text.ToLower().Contains(searchTerm.ToLower());
}