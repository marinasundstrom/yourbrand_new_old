namespace Carts.API.Extensions;

public static class OpenApiExtensions
{
	public static IServiceCollection AddOpenApi(this IServiceCollection services)
	{
        services.AddEndpointsApiExplorer();
        services.AddOpenApiDocument(config => {
            config.PostProcess = document =>
            {
                document.Info.Title = "Carts API";
            };

            config.DefaultReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
        });

        return services;
	}

    public static WebApplication UseOpenApi(this WebApplication app)
    {
        app.UseOpenApi(p => p.Path = "/swagger/{documentName}/swagger.yaml");
        app.UseSwaggerUi3(p => p.DocumentPath = "/swagger/{documentName}/swagger.yaml");

        return app;
    }
}

