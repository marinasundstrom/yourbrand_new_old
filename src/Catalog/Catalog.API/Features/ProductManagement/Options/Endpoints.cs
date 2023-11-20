
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Catalog.API.Model;

using Asp.Versioning.Builder;

namespace Catalog.API.Features.ProductManagement.Options;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapOptionsEndpoints(this IEndpointRouteBuilder app)
    {
        var versionedApi = app.NewVersionedApi("Options");

        var group = versionedApi.MapGroup("/v{version:apiVersion}/options")
            .WithTags("Attributes")
            .HasApiVersion(1, 0)
            .WithOpenApi();

        group.MapGet("/", GetOptionValues)
            .WithName($"Options_{nameof(GetOptions)}");

        group.MapGet("/{id}/values", GetOptionValues)
            .WithName($"Options_{nameof(GetOptionValues)}");

        return app;
    }

    public static async Task<Results<Ok<IEnumerable<OptionDto>>, BadRequest>> GetOptions(bool includeChoices = false, IMediator mediator = default)
    {
        return TypedResults.Ok(await mediator.Send(new GetOptions(includeChoices)));
    }

    /*
    [HttpGet("{optionId}")]
    public async Task<IResult<OptionDto>> GetProductOptionValues(string optionId, IMediator mediator = default)
    {
        return TypedResults.Ok(await _mediator.Send(new GetOption(optionId)));
    }
    */

    public static async Task<Results<Ok<IEnumerable<OptionValueDto>>, BadRequest>> GetOptionValues(string id, IMediator mediator = default)
    {
        return TypedResults.Ok(await mediator.Send(new GetOptionValues(id)));
    }
}