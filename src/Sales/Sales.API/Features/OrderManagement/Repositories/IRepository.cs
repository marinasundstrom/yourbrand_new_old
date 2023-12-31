﻿using YourBrand.Sales.API.Features.OrderManagement.Domain.Entities;

using YourBrand.Sales.API.Features.OrderManagement.Domain.Specifications;

namespace YourBrand.Sales.API.Features.OrderManagement.Repositories;

public interface IRepository<T, TId>
    where T : AggregateRoot<TId>
    where TId : notnull
{
    IQueryable<T> GetAll();
    IQueryable<T> GetAll(ISpecification<T> specification);
    Task<T?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);
    void Add(T entity);
    void Remove(T entity);
}