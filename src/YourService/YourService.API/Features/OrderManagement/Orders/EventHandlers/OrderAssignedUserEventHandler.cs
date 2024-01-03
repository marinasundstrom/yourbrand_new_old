using YourBrand.YourService.API.Domain.Events;
using YourBrand.YourService.API.Features.OrderManagement.Repositories;

using YourBrand.YourService.API.Common;
using YourBrand.YourService.API.Services;

namespace YourBrand.YourService.API.Features.OrderManagement.Orders.EventHandlers;

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