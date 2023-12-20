using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Sales.API.Features.OrderManagement.Domain.Entities;

namespace Sales.API.Features.OrderManagement.Domain;

public interface ISalesContext
{
    DbSet<OrderStatus> OrderStatuses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}