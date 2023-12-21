using Sales.API.Features.OrderManagement.Domain.Events;
using Sales.API.Features.OrderManagement.Repositories;

using YourBrand.Orders.Application.Common;
using YourBrand.Orders.Application.Services;

namespace Sales.API.Features.OrderManagement.Orders.EventHandlers;

public sealed class OrderAssignedUserEventHandler : IDomainEventHandler<OrderAssignedUserUpdated>
{
    private readonly IOrderRepository orderRepository;
    private readonly IEmailService emailService;
    private readonly IOrderNotificationService orderNotificationService;

    public OrderAssignedUserEventHandler(IOrderRepository orderRepository, IEmailService emailService, IOrderNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.emailService = emailService;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(OrderAssignedUserUpdated notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindByIdAsync(notification.OrderId, cancellationToken);

        if (order is null)
            return;

        if (order.AssigneeId is not null && order.LastModifiedById != order.AssigneeId)
        {
            /*
            await emailService.SendEmail(
                order.AssigneeId!.Email,
                $"You were assigned to \"{order.Title}\" [{order.OrderNo}].",
                $"{order.LastModifiedBy!.Name} assigned {order.AssigneeId.Name} to \"{order.Title}\" [{order.OrderNo}]."); */
        }
    }
}