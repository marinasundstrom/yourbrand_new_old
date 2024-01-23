using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API;
using YourBrand.YourService.API.Repositories;
using YourBrand.YourService.API.Common;
using YourBrand.YourService.API.Domain.Specifications;
using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Features.Todos;

public sealed record GetTodos(int Page = 1, int PageSize = 10, string? SearchTerm = null, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<PagedResult<TodoDto>>
{
    public sealed class Handler(ITodoRepository todoRepository) : IRequestHandler<GetTodos, PagedResult<TodoDto>>
    {
        private readonly ITodoRepository todoRepository = todoRepository;

        public async Task<PagedResult<TodoDto>> Handle(GetTodos request, CancellationToken cancellationToken)
        {
            var specification = new IsCompleted(false).And(new HasExpired(TimeSpan.FromDays(30)));

            var query = todoRepository.Find(specification);

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SearchTerm is not null)
            {
                query = query.Where(x => x.Text.ToLower().Contains(request.SearchTerm.ToLower()));
            }

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
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new PagedResult<TodoDto>(todos.Select(todo => todo.ToDto()), totalCount);
        }
    }
}

public record TodoDto(string Id, string Text);