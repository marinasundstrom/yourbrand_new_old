using FluentValidation;

using MediatR;

using YourBrand.YourService.API.Behaviors;
using YourBrand.YourService.API.Common;

namespace YourBrand.YourService.API;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining(typeof(ServiceExtensions));

            //x.NotificationPublisherType = typeof(IdempotentDomainEventPublisher);
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);

        //services.AddOrderManagement();

        return services;
    }

    public static IServiceCollection AddTenantService(this IServiceCollection services)
    {
        services.AddScoped<ITenantService, TenantService>();

        return services;
    }

    public static IServiceCollection AddCurrentUserService(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}