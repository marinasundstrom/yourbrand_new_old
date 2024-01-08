using Microsoft.AspNetCore.Http.HttpResults;

using YourBrand.Catalog;
using YourBrand.StoreFront.API.Features.Products;

namespace YourBrand.StoreFront.API.Features.Brands;

public static class Endpoints
{
    private static string? storeId = null;

    public static IEndpointRouteBuilder MapBrandsEndpoints(this IEndpointRouteBuilder app)
    {
        var versionedApi = app.NewVersionedApi("Brands");

        var productsGroup = versionedApi.MapGroup("/v{version:apiVersion}/brands")
            .WithTags("Brands")
            .HasApiVersion(1, 0)
            .WithOpenApi()
            .RequireCors();

        productsGroup.MapGet("/", GetBrands)
            .WithName($"Brands_{nameof(GetBrands)}");

        return app;
    }

    private static async Task<Results<Ok<PagedResult<Brand>>, NotFound>> GetBrands(string? productCategoryId = null, int? page = 1, int? pageSize = 10, string? searchTerm = null, IBrandsClient brandsClient = default!, IStoresClient storesClient = default!, CancellationToken cancellationToken = default)
    {
        if (storeId is null)
        {
            var store = await storesClient.GetStoreByIdAsync("my-store");
            storeId = store.Id;
        }

        var results = await brandsClient.GetBrandsAsync(productCategoryId, page, pageSize, searchTerm, null, null, cancellationToken);
        return results is not null ? TypedResults.Ok(
                new PagedResult<Brand>(results.Items.Select(x => x.Map()), results.Total)
        ) : TypedResults.NotFound();
    }
}