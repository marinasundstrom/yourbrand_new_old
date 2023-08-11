using BlazorApp.ProductCategories;

namespace Client.ProductCategories;

public static class ServiceExtensions
{
    public static IServiceCollection AddProductCategoriesServices(this IServiceCollection services) 
    {
        services.AddScoped<IProductCategoryService, ProductCategoryService>();

         services.AddHttpClient<StoreWeb.IProductCategoriesClient>("WebAPI")
            .AddTypedClient<StoreWeb.IProductCategoriesClient>((http, sp) => new StoreWeb.ProductCategoriesClient(http));
        
        return services;
    }
}