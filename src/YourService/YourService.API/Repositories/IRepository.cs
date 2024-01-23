using YourBrand.YourService.API.Domain.Entities;

using YourBrand.YourService.API.Domain.Specifications;

namespace YourBrand.YourService.API.Repositories;

public interface IRepository<T, TId>
    where T : AggregateRoot<TId>
    where TId : notnull
{
    IQueryable<T> GetAll();
    IQueryable<T> Find(Specification<T> specification);
    Task<T?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);
    void Add(T entity);
    void Remove(T entity);
}