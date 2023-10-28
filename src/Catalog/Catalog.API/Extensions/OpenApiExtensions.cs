using Asp.Versioning;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using NJsonSchema.Generation;

namespace Catalog.API.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        var apiVersionDescriptions = new[] {
            (ApiVersion: new ApiVersion(1, 0), foo: 1),
            (ApiVersion: new ApiVersion(2, 0), foo: 1)
        };

        foreach (var description in apiVersionDescriptions)
        {
            services.AddOpenApiDocument(config =>
            {
                config.DocumentName = $"v{GetApiVersion(description)}";
                config.PostProcess = document =>
                {
                    document.Info.Title = "Catalog API";
                    document.Info.Version = $"v{GetApiVersion(description)}";
                };
                config.ApiGroupNames = new[] { GetApiVersion(description) };

                config.DefaultReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;

                config.AddSecurity("JWT", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));

                config.SchemaNameGenerator = new CustomSchemaNameGenerator();
            });
        }

        static string GetApiVersion((ApiVersion ApiVersion, int foo) description)
        {
            var apiVersion = description.ApiVersion;
            return (apiVersion.MinorVersion == 0
                ? apiVersion.MajorVersion.ToString()
                : apiVersion.ToString())!;
        }

        return services;
    }

    public static WebApplication UseOpenApi(this WebApplication app)
    {
        app.UseOpenApi(p => p.Path = "/swagger/{documentName}/swagger.yaml");
        app.UseSwaggerUi3(options =>
        {
            var descriptions = app.DescribeApiVersions();

            // build a swagger endpoint for each discovered API version
            foreach (var description in descriptions)
            {
                var name = $"v{description.ApiVersion}";
                var url = $"/swagger/v{GetApiVersion(description)}/swagger.yaml";

                options.SwaggerRoutes.Add(new SwaggerUi3Route(name, url));
            }
        });

        static string GetApiVersion(Asp.Versioning.ApiExplorer.ApiVersionDescription description)
        {
            var apiVersion = description.ApiVersion;
            return (apiVersion.MinorVersion == 0
                ? apiVersion.MajorVersion.ToString()
                : apiVersion.ToString())!;
        }

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