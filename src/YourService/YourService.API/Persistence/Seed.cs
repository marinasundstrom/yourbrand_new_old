using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Persistence;

public static class Seed
{
    public static async Task SeedData(AppDbContext context, IConfiguration configuration)
    {
        // Seed data here

        await context.SaveChangesAsync();
    }
}