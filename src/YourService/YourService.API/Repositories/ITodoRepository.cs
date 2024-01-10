using YourBrand.YourService.API.Domain.Entities;

using YourBrand.YourService.API.Domain.Specifications;

namespace YourBrand.YourService.API.Repositories;

public interface ITodoRepository : IRepository<Todo, string>
{
    //Task<Todo?> FindByNoAsync(int orderNo, CancellationToken cancellationToken = default);
}