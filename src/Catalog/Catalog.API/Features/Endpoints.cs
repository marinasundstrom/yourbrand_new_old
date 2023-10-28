using Catalog.API.Features.Brands;
using Catalog.API.Features.Currencies;
using Catalog.API.Features.ProductManagement;
using Catalog.API.Features.ProductManagement.ProductCategories;
using Catalog.API.Features.ProductManagement.Products;
using Catalog.API.Features.Stores;

namespace Catalog.API.Features;

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