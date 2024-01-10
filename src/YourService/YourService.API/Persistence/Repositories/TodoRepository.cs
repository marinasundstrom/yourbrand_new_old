using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.Specifications;
using YourBrand.YourService.API.Repositories;
using YourBrand.YourService.API.Persistence;

namespace YourBrand.YourService.API.Persistence.Repositories.Mocks;

public sealed class TodoRepository : ITodoRepository
{
    readonly AppDbContext context;
    readonly DbSet<Todo> dbSet;

    public TodoRepository(AppDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<Todo>();
    }

    public IQueryable<Todo> GetAll()
    {
        //return dbSet.Where(new OrdersWithStatus(OrderStatus.Completed).Or(new OrdersWithStatus(OrderStatus.OnHold))).AsQueryable();

        return dbSet.AsQueryable();
    }

    public async Task<Todo?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public IQueryable<Todo> GetAll(ISpecification<Todo> specification)
    {
        return dbSet
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .Where(specification.Criteria);
    }

    public void Add(Todo item)
    {
        dbSet.Add(item);
    }

    public void Remove(Todo item)
    {
        dbSet.Remove(item);
    }
}