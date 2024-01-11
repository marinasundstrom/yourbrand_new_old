using MassTransit;

public static class ServiceBusExtensions
{
    public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumers(typeof(Program).Assembly);

            if (environment.IsProduction())
            {
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host($"sb://{configuration["Azure:ServiceBus:Namespace"]}.servicebus.windows.net");

                    cfg.ConfigureEndpoints(context);
                });
            }
            else
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitmqHost = configuration["RABBITMQ_HOST"] ?? "localhost";

                    cfg.Host(rabbitmqHost, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            }
        });

        return services;
    }
}