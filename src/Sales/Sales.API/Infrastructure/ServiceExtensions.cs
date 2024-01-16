using MediatR;

using Quartz;

using YourBrand.Sales.API.Infrastructure.BackgroundJobs;
using YourBrand.Sales.API.Infrastructure.Idempotence;
using YourBrand.Sales.API.Infrastructure.Services;
using YourBrand.Sales.API.Persistence;

using Scrutor;

using YourBrand.Orders.Application.Common;

namespace YourBrand.Sales.API.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IEmailService, EmailService>();

        foreach (var reg in services.Where(reg => reg.ServiceType.Name.Contains("INotificationHandler")).ToList())
        {
            var notificationHandlerType = reg.ServiceType!;
            var notificationHandlerImplType = reg.ImplementationType!;

            var requestType = notificationHandlerType.GetGenericArguments().FirstOrDefault();

            if (!notificationHandlerImplType.Name.Contains("IdempotentDomainEventHandler")) continue;

            services.Remove(reg);
        }

        try
        {
            services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
        }
        catch { }

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(10)
                        .RepeatForever()));

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService();

        return services;
    }
}