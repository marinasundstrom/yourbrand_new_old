using YourBrand.Domain.Infrastructure;
using YourBrand.YourService.API.Infrastructure.Services;

namespace YourBrand.YourService.API.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<IEmailService, EmailService>();

        services.AddDomainInfrastructure(configuration);

        return services;
    }
}