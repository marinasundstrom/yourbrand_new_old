using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.Specifications;

namespace YourBrand.YourService.API.Features.Todos;

public class HasExpired(TimeSpan expirationThreshold) : Specification<Todo>
{
    public override Expression<Func<Todo, bool>> ToExpression()
    {
        int days = expirationThreshold.Days;
        return todo => todo.Created.AddDays(days) < DateTimeOffset.UtcNow;
    }
}
