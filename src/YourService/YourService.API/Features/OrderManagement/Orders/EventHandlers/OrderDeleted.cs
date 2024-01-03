using YourBrand.YourService.API.Domain.Events;
using YourBrand.YourService.API.Features.OrderManagement.Repositories;

using YourBrand.YourService.API.Common;
using YourBrand.YourService.API.Services;

namespace YourBrand.YourService.API.Features.OrderManagement.Orders.EventHandlers;

public sealed class OrderDeletedEventHandler : IDomainEventHandler<OrderDeleted>
{
    private readonly IOrderRepository orderRepository;
    private readonly IOrderNotificationService orderNotificationService;

    public OrderDeletedEventHandler(IOrderRepository orderRepository, IOrderNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(OrderDeleted notification, CancellationToken cancellationToken)
    {
        await orderNotificationService.Deleted(notification.OrderNo);
    }
}