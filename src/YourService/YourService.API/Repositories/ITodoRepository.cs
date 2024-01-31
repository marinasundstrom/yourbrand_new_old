using YourBrand.YourService.API.Domain.Entities;

using YourBrand.YourService.API.Domain.Specifications;
using YourBrand.YourService.API.Domain.ValueObjects;

namespace YourBrand.YourService.API.Repositories;

public interface ITodoRepository : IRepository<Todo, TodoId>
{
    Task<int> RemoveByIdAsync(TodoId id);
}