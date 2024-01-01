using YourBrand.Catalog.API.Features.ProductManagement.Products.Images;

namespace YourBrand.Catalog.API.Features.ProductManagement.Products;

public static class ServicesExtensions
{
    public static IServiceCollection AddProductsServices(this IServiceCollection services)
    {
        services.AddScoped<IProductImageUploader, ProductImageUploader>();

        return services;
    }
}