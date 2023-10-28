
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Catalog.API.Model;

namespace Catalog.API.Features.Currencies;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapCurrenciesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/currencies")
            .WithTags("Currencies")
            .RequireRateLimiting("fixed")
            .WithOpenApi();

        group.MapGet("/", GetCurrencies)
            .WithName($"Currencies_{nameof(GetCurrencies)}");

        return app;
    }

    public static async Task<PagedResult<CurrencyDto>> GetCurrencies(int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, API.SortDirection? sortDirection = null, IMediator mediator = default, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetCurrenciesQuery(page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }
}