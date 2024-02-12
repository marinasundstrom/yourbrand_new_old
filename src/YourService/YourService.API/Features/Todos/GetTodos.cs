using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API;
using YourBrand.YourService.API.Repositories;
using YourBrand.YourService.API.Common;
using YourBrand.YourService.API.Domain.Specifications;
using YourBrand.YourService.API.Domain.Entities;
using MassTransit.Internals;

namespace YourBrand.YourService.API.Features.Todos;

public sealed record GetTodos(bool? IsCompleted, int Page = 1, int PageSize = 10, string? SearchTerm = null, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<Result<PagedResult<TodoDto>>>
{
    public sealed class Handler(ITodoRepository todoRepository, IConfiguration configuration) : IRequestHandler<GetTodos, Result<PagedResult<TodoDto>>>
    {
        private readonly ITodoRepository todoRepository = todoRepository;

        public async Task<Result<PagedResult<TodoDto>>> Handle(GetTodos request, CancellationToken cancellationToken)
        {
            Result<Specification<Todo>> specificationResult = CreateSpecification(configuration, request);

            //Console.WriteLine(specificationResult.GetValue().ToExpression().ToCSharpString());

            if (specificationResult.HasError(out Error error))
            {
                return error;
            }

            var specification = specificationResult.GetValue();

            var query = todoRepository.Find(specification);

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }
            else
            {
                query = query.OrderByDescending(todo => todo.Created);
            }

            var todos = await query
                .OrderBy(todo => todo.Id)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            return new PagedResult<TodoDto>(todos.Select(todo => todo.ToDto()), totalCount);
        }

        private static Result<Specification<Todo>> CreateSpecification(IConfiguration configuration, GetTodos request)
        {
            var specification = Specification<Todo>.All;

            if (request.IsCompleted is not null)
            {
                specification = specification.And(new IsCompletedSpecification(request.IsCompleted.GetValueOrDefault()));
            }

            if (request.SearchTerm is not null)
            {
                if (string.IsNullOrEmpty(request.SearchTerm))
                {
                    return Errors.SearchTermIsEmpty;
                }

                specification = specification.And(new MatchesSearchTerm(request.SearchTerm));
            }

            return specification;
        }
    }
}

public record TodoDto(string Id, string Text, bool IsCompleted);