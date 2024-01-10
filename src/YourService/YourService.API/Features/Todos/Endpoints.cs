using MassTransit;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using YourBrand.YourService.API.Common;

namespace YourBrand.YourService.API.Features.Todos;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapTodosEndpoints(this IEndpointRouteBuilder app)
    {
        string GetOrdersExpire20 = nameof(GetOrdersExpire20);

        var versionedApi = app.NewVersionedApi("Todos");

        var group = versionedApi.MapGroup("/v{version:apiVersion}/todos")
            .WithTags("Todos")
            .HasApiVersion(ApiVersions.V1)
            .WithOpenApi();

        group.MapGet("/", GetTodos)
            .WithName($"Users{nameof(GetTodos)}")
            .Produces<PagedResult<TodoDto>>(StatusCodes.Status200OK);

        return app;
    }

    public static async Task<PagedResult<TodoDto>> GetTodos(int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, SortDirection? sortDirection = null,
        IMediator mediator = default!, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetTodos(page, pageSize, searchTerm, sortBy, sortDirection), cancellationToken);


}