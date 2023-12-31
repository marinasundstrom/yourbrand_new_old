using YourBrand.Catalog.API.Features.Brands;
using YourBrand.Catalog.API.Features.Currencies;
using YourBrand.Catalog.API.Features.ProductManagement;
using YourBrand.Catalog.API.Features.ProductManagement.ProductCategories;
using YourBrand.Catalog.API.Features.ProductManagement.Products;
using YourBrand.Catalog.API.Features.Stores;

namespace YourBrand.Catalog.API.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapFeaturesEndpoints(this IEndpointRouteBuilder app)
    {
        app
        .MapBrandsEndpoints()
        .MapCurrenciesEndpoints()
        .MapProductManagementEndpoints()
        .MapStoresEndpoints();

        return app;
    }
}