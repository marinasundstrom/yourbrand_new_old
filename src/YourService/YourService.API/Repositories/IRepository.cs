using YourBrand.YourService.API.Domain.Entities;

using YourBrand.YourService.API.Domain.Specifications;

namespace YourBrand.YourService.API.Features.OrderManagement.Repositories;

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