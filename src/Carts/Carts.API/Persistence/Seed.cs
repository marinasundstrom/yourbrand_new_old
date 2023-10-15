using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Carts.API.Domain.Entities;

namespace Carts.API.Persistence;

public static class Seed
{
    public static async Task SeedData(CartsContext context, IConfiguration configuration)
    {
        //await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        context.Carts.Add(new Cart("test", "Test"));

        await context.SaveChangesAsync();
    }
}