﻿using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.Specifications;
using YourBrand.YourService.API.Features.OrderManagement.Repositories;
using YourBrand.YourService.API.Persistence;

namespace YourBrand.YourService.API.Persistence.Repositories.Mocks;

public sealed class OrderRepository : IOrderRepository
{
    readonly AppDbContext context;
    readonly DbSet<Order> dbSet;

    public OrderRepository(AppDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<Order>();
    }

    public IQueryable<Order> GetAll()
    {
        //return dbSet.Where(new OrdersWithStatus(OrderStatus.Completed).Or(new OrdersWithStatus(OrderStatus.OnHold))).AsQueryable();

        return dbSet.AsQueryable();
    }

    public async Task<Order?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.Status)
            .Include(i => i.Items)
            .Include(i => i.Assignee)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<Order?> FindByNoAsync(int orderNo, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.Status)
            .Include(i => i.Items)
            .Include(i => i.Assignee)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.OrderNo.Equals(orderNo), cancellationToken);
    }

    public IQueryable<Order> GetAll(ISpecification<Order> specification)
    {
        return dbSet
            .Include(i => i.Status)
            .Include(i => i.Items)
            .Include(i => i.Assignee)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .Where(specification.Criteria);
    }

    public void Add(Order item)
    {
        dbSet.Add(item);
    }

    public void Remove(Order item)
    {
        dbSet.Remove(item);
    }
}