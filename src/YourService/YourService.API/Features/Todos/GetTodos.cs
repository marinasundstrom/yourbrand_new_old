using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API;
using YourBrand.YourService.API.Repositories;
using YourBrand.YourService.API.Common;
using YourBrand.YourService.API.Domain.Specifications;
using YourBrand.YourService.API.Domain.Entities;
using MassTransit.Internals;

namespace YourBrand.YourService.API.Features.Todos;

public sealed record GetTodos(bool? IsCompleted, bool? HasExpired, int Page = 1, int PageSize = 10, string? SearchTerm = null, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<PagedResult<TodoDto>>
{
    public sealed class Handler(ITodoRepository todoRepository, IConfiguration configuration) : IRequestHandler<GetTodos, PagedResult<TodoDto>>
    {
        private readonly ITodoRepository todoRepository = todoRepository;

        public async Task<PagedResult<TodoDto>> Handle(GetTodos request, CancellationToken cancellationToken)
        {
            var specification = Specification<Todo>.All;

            if (request.IsCompleted ?? false)
            {
                specification = specification.And(new IsCompleted());
            }

            if (request.HasExpired ?? false)
            {
                var expirationThreshold2 = configuration.GetValue<int?>("TodoExpirationThreshold") ?? 5;

                specification = specification.And(new HasExpired(expirationThreshold: TimeSpan.FromDays(expirationThreshold2)));
            }

            if (request.SearchTerm is not null)
            {
                specification = specification.And(new MatchesSearchTerm(request.SearchTerm));
            }

            Console.WriteLine(specification.ToExpression().ToCSharpString());

            var query = todoRepository.Find(specification);

            Console.WriteLine(query.ToQueryString());

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }
            else
            {
                query = query.OrderByDescending(x => x.Created);
            }

            var todos = await query
                .OrderBy(i => i.Id)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new PagedResult<TodoDto>(todos.Select(todo => todo.ToDto()), totalCount);
        }
    }
}

public record TodoDto(string Id, string Text);