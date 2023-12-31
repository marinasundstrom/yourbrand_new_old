
using Asp.Versioning.Builder;

using YourBrand.Catalog.API.Model;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace YourBrand.Catalog.API.Features.Currencies;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapCurrenciesEndpoints(this IEndpointRouteBuilder app)
    {
        var versionedApi = app.NewVersionedApi("Currencies");

        var group = versionedApi.MapGroup("/v{version:apiVersion}/currencies")
            .WithTags("Currencies")
            .HasApiVersion(1, 0)
            .WithOpenApi()
            .RequireAuthorization();

        group.MapGet("/", GetCurrencies)
            .WithName($"Currencies_{nameof(GetCurrencies)}");

        return app;
    }

    public static async Task<PagedResult<CurrencyDto>> GetCurrencies(int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, API.SortDirection? sortDirection = null, IMediator mediator = default, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetCurrenciesQuery(page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }
}