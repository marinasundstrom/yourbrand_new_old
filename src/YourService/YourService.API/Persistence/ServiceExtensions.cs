using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.YourService.API.Features.OrderManagement.Repositories;
using YourBrand.YourService.API.Persistence.Interceptors;
using YourBrand.YourService.API.Persistence.Repositories;
using YourBrand.YourService.API.Persistence.Repositories.Mocks;

namespace YourBrand.YourService.API.Persistence;

public static class ServiceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        const string ConnectionStringKey = "mssql";

        var connectionString = Infrastructure.ConfigurationExtensions.GetConnectionString(configuration, ConnectionStringKey, "Orders")
            ?? configuration.GetValue<string>("yourbrand:sales-svc:db:connectionstring");



        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString!, o => o.EnableRetryOnFailure());

            options.AddInterceptors(
                sp.GetRequiredService<OutboxSaveChangesInterceptor>(),
                sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());

#if DEBUG
            options
                //.LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging();
#endif
        });

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<OutboxSaveChangesInterceptor>();

        RegisterRepositories(services);

        return services;
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        // TODO: Automate this

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}