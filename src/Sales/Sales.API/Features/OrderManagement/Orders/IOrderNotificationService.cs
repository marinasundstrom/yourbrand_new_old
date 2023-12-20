using Sales.API.Features.OrderManagement.Orders.Dtos;

namespace Sales.API.Features.OrderManagement.Orders;

public interface IOrderNotificationService
{
    Task Created(int orderNo);

    Task Updated(int orderNo);

    Task Deleted(int orderNo);

    Task StatusUpdated(int orderNo, OrderStatusDto status);
}