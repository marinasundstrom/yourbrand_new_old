using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Domain;

public interface IAppDbContext
{
    DbSet<OrderStatus> OrderStatuses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}