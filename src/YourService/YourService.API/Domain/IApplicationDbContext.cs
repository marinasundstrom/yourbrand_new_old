using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Domain;

public interface IApplicationDbContext
{
    DbSet<Todo> Todos { get; }

    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}