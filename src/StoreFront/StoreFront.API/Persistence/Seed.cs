using StoreFront.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace StoreFront.API.Persistence;

public static class Seed
{
    public static async Task SeedData(StoreFrontContext context, IConfiguration configuration)
    {
        //await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        context.StoreFront.Add(new Cart("test", "Test"));

        await context.SaveChangesAsync();
    }
}