using MediatR;

using YourBrand.YourService.API.Domain;

namespace YourBrand.YourService.API.Common;

public interface IDomainEventHandler<TDomainEvent>
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{

}