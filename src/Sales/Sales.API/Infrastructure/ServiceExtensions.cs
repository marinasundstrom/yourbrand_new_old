﻿using YourBrand.Sales.API.Infrastructure.Services;

using YourBrand.Domain.Infrastructure;

namespace YourBrand.Sales.API.Infrastructure;

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