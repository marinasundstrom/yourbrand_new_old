using NJsonSchema.Generation;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApiDocument(config =>
        {
            config.PostProcess = document =>
            {
                document.Info.Title = "Catalog API";
            };

            config.DefaultReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;

            config.SchemaNameGenerator = new CustomSchemaNameGenerator();
        });

        return services;
    }

    public static WebApplication UseOpenApi(this WebApplication app)
    {
        app.UseOpenApi(p => p.Path = "/swagger/{documentName}/swagger.yaml");
        app.UseSwaggerUi3(p => p.DocumentPath = "/swagger/{documentName}/swagger.yaml");

        return app;
    }

    public class CustomSchemaNameGenerator : ISchemaNameGenerator
    {
        public string Generate(Type type)
        {
            if (type.IsGenericType)
            {
                return $"{type.Name.Replace("`1", string.Empty)}Of{GenerateName(type.GetGenericArguments().First())}";
            }
            return GenerateName(type);
        }

        private static string GenerateName(Type type)
        {
            return type.Name
                .Replace("Dto", string.Empty)
                .Replace("Command", string.Empty)
                .Replace("Query", string.Empty);
        }
    }
}