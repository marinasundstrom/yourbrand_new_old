using YourBrand.Sales.API.Features.OrderManagement.Domain;

namespace YourBrand.Orders.Application.Services;

public interface IDomainEventDispatcher
{
    Task Dispatch(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}