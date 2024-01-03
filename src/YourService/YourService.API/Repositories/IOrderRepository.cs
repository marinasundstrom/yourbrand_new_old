using YourBrand.YourService.API.Domain.Entities;

using YourBrand.YourService.API.Domain.Specifications;

namespace YourBrand.YourService.API.Features.OrderManagement.Repositories;

public interface IOrderRepository : IRepository<Order, string>
{
    Task<Order?> FindByNoAsync(int orderNo, CancellationToken cancellationToken = default);
}