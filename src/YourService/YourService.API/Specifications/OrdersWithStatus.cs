using System;

using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Domain.Specifications;

public class OrdersWithStatus : BaseSpecification<Order>
{
    public OrdersWithStatus(OrderStatus status)
    {
        Criteria = order => order.Status == status;
    }
}