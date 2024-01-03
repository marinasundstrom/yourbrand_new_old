using YourBrand.YourService.API.Features.OrderManagement.Orders.Dtos;

namespace YourBrand.YourService.API.Features.OrderManagement.Orders;

public interface IOrdersHubClient
{
    Task Created(int orderNo);

    Task Updated(int orderNo);

    Task Deleted(int orderNo);

    Task StatusUpdated(int orderNo, OrderStatusDto status);
}