using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Infrastructure.Persistence.Repositories.Mocks;

public static class Includes
{
    public static IQueryable<Todo> IncludeAll(this IQueryable<Todo> query) => query
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy);
}