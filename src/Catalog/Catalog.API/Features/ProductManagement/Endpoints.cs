using Catalog.API.Features.ProductManagement.ProductCategories;
using Catalog.API.Features.ProductManagement.Products;

namespace Catalog.API.Features.ProductManagement;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapProductManagementEndpoints(this IEndpointRouteBuilder app)
    {
        app
        .MapProductsEndpoints()
        .MapProductCategoriesEndpoints();

        return app;
    }
}