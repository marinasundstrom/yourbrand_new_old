﻿using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API;
using YourBrand.YourService.API.Common;
using YourBrand.YourService.API.Repositories;

namespace YourBrand.YourService.API.Features.Users;

public sealed record GetUsers(int Page = 1, int PageSize = 10, string? SearchTerm = null, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<PagedResult<UserDto>>
{
    public sealed class Handler : IRequestHandler<GetUsers, PagedResult<UserDto>>
    {
        private readonly IUserRepository userRepository;

        public Handler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<PagedResult<UserDto>> Handle(GetUsers request, CancellationToken cancellationToken)
        {
            var query = userRepository.GetAll();

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SearchTerm is not null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.SearchTerm.ToLower()));
            }

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }
            else
            {
                query = query.OrderByDescending(x => x.Created);
            }

            var users = await query
                .OrderBy(i => i.Id)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new PagedResult<UserDto>(users.Select(x => x.ToDto()), totalCount);
        }
    }
}