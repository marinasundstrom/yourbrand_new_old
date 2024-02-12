namespace YourBrand.YourService.API.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context, IConfiguration configuration)
    {
        // Seed data here

        await context.SaveChangesAsync();
    }
}