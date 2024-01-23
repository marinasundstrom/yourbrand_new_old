using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.Specifications;
using YourBrand.YourService.API.Repositories;
using YourBrand.YourService.API.Persistence;

namespace YourBrand.YourService.API.Persistence.Repositories.Mocks;

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

    public async Task<Todo?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
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

    public async Task<int> RemoveByIdAsync(string id)
    {
        return await dbSet.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}
