using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.Specifications;
using YourBrand.YourService.API.Repositories;
using YourBrand.YourService.API.Infrastructure.Persistence;
using YourBrand.YourService.API.Domain.ValueObjects;

namespace YourBrand.YourService.API.Infrastructure.Persistence.Repositories.Mocks;

public sealed class TodoRepository : ITodoRepository
{
    readonly DbSet<Todo> dbSet;

    public TodoRepository(ApplicationDbContext context)
    {
        dbSet = context.Set<Todo>();
    }

    public IQueryable<Todo> GetAll()
    {
        return dbSet
            .IncludeAll()
            .AsQueryable();
    }

    public IQueryable<Todo> Find(Specification<Todo> specification)
    {
        return dbSet
            .IncludeAll()
            .Where(specification.ToExpression());
    }

    public async Task<Todo?> FindByIdAsync(TodoId id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .IncludeAll()
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public void Add(Todo item)
    {
        dbSet.Add(item);
    }

    public void Remove(Todo item)
    {
        dbSet.Remove(item);
    }

    public async Task<int> RemoveByIdAsync(TodoId id)
    {
        return await dbSet.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}
