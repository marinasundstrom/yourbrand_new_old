using System;

using Sales.API.Features.OrderManagement.Domain.Entities;

namespace Sales.API.Features.OrderManagement.Domain.Specifications;

public class OrdersWithStatus : BaseSpecification<Order>
{
    public OrdersWithStatus(OrderStatus status)
    {
        Criteria = order => order.Status == status;
    }
}