using MediatR;

using Sales.API.Features.OrderManagement.Domain;

namespace YourBrand.Orders.Application.Common;

public interface IDomainEventHandler<TDomainEvent>
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{

}