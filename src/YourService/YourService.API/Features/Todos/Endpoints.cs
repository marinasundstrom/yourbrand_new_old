using FluentValidation;

using MassTransit;

using MediatR;

using Microsoft.AspNetCore.Http.HttpResults;
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
            .WithName($"Todos_{nameof(GetTodos)}")
            .Produces<PagedResult<TodoDto>>(StatusCodes.Status200OK);
        //.RequireAuthorization();

        group.MapPost("/", CreateTodo)
            .WithName($"Todos_{nameof(CreateTodo)}")
            .Produces<TodoDto>(StatusCodes.Status200OK)
            .AddEndpointFilter<ValidationFilter<CreateTodoRequest>>();
        //.RequireAuthorization();

        group.MapPost("/{id}", MarkTodoAsCompleted)
            .WithName($"Todos_{nameof(MarkTodoAsCompleted)}")
            .Produces(StatusCodes.Status200OK)
            .AddEndpointFilter<ValidationFilter<MarkTodoAsCompletedRequest>>();
        //.RequireAuthorization();

        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }

    public static async Task<Results<Ok<PagedResult<TodoDto>>, ProblemHttpResult>> GetTodos(bool? isCompleted, int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, SortDirection? sortDirection = null,
        IMediator mediator = default!, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetTodos(isCompleted, page, pageSize, searchTerm, sortBy, sortDirection), cancellationToken);

        return result.Match<Results<Ok<PagedResult<TodoDto>>, ProblemHttpResult>>(
            pagedResult => TypedResults.Ok(pagedResult),
            error => TypedResults.Problem(error.Detail, title: error.Title));
    }

    public static async Task<TodoDto> CreateTodo(CreateTodoRequest request, IMediator mediator = default!, CancellationToken cancellationToken = default)
        => (await mediator.Send(new CreateTodo(request.Text), cancellationToken)).GetValue();

    public static async Task MarkTodoAsCompleted(Guid id, MarkTodoAsCompletedRequest request, IMediator mediator = default!, CancellationToken cancellationToken = default)
        => await mediator.Send(new MarkTodoAsCompleted(id), cancellationToken);
}

public record CreateTodoRequest(string Text)
{
    public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
    {
        public CreateTodoRequestValidator()
        {
            RuleFor(p => p.Text).MaximumLength(120).NotEmpty();
        }
    }
}

public record MarkTodoAsCompletedRequest()
{
    public class MarkTodoAsCompletedRequestValidator : AbstractValidator<MarkTodoAsCompletedRequest>
    {
        public MarkTodoAsCompletedRequestValidator()
        {

        }
    }
}