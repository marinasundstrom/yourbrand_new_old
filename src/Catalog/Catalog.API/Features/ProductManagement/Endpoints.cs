using YourBrand.Catalog.API.Features.ProductManagement.Attributes;
using YourBrand.Catalog.API.Features.ProductManagement.Options;
using YourBrand.Catalog.API.Features.ProductManagement.ProductCategories;
using YourBrand.Catalog.API.Features.ProductManagement.Products;

namespace YourBrand.Catalog.API.Features.ProductManagement;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapProductManagementEndpoints(this IEndpointRouteBuilder app)
    {
        app
        .MapProductsEndpoints()
        .MapProductCategoriesEndpoints()
        .MapAttributesEndpoints()
        .MapOptionsEndpoints();

        return app;
    }
}