using YourBrand.YourService.API.Domain;

namespace YourBrand.YourService.API.Services;

public interface IDomainEventDispatcher
{
    Task Dispatch(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}