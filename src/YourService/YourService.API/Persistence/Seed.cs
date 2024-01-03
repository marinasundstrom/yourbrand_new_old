using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Persistence;

public static class Seed
{
    public static async Task SeedData(AppDbContext context, IConfiguration configuration)
    {
        Version1(context);

        await context.SaveChangesAsync();
    }

    private static void Version1(AppDbContext context)
    {
        context.OrderStatuses.Add(new OrderStatus("Draft", "draft", string.Empty));

        context.OrderStatuses.Add(new OrderStatus("Open", "open", string.Empty));
        context.OrderStatuses.Add(new OrderStatus("Archived", "archived", string.Empty));
        context.OrderStatuses.Add(new OrderStatus("Canceled", "canceled", string.Empty));
    }
}