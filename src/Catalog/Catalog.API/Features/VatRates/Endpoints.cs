
using Asp.Versioning.Builder;

using YourBrand.Catalog.API.Model;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace YourBrand.Catalog.API.Features.VatRates;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapVatRatesEndpoints(this IEndpointRouteBuilder app)
    {
        var versionedApi = app.NewVersionedApi("VatRates");

        var group = versionedApi.MapGroup("/v{version:apiVersion}/vatRates")
            .WithTags("VatRates")
            .HasApiVersion(ApiVersions.V1)
            .WithOpenApi()
            .RequireAuthorization();

        group.MapGet("/", GetVatRates)
            .WithName($"VatRates_{nameof(GetVatRates)}");

        return app;
    }

    public static async Task<PagedResult<VatRateDto>> GetVatRates(int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, API.SortDirection? sortDirection = null, IMediator mediator = default, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetVatRatesQuery(page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }
}