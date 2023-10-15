using Catalog.API.Domain.Entities;
using Catalog.API.Model;
using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products.Variants;

public record GetProductVariants(string ProductIdOrHandle, int Page = 10, int PageSize = 10, string? SearchString = null, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<PagedResult<Catalog.API.Features.ProductManagement.Products.ProductDto>>
{
    public class Handler : IRequestHandler<GetProductVariants, PagedResult<Catalog.API.Features.ProductManagement.Products.ProductDto>>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Catalog.API.Features.ProductManagement.Products.ProductDto>> Handle(GetProductVariants request, CancellationToken cancellationToken)
        {
            bool isProductId = long.TryParse(request.ProductIdOrHandle, out var productId);

            var query = _context.Products.AsQueryable();

            query = isProductId ?
                query.Where(pv => pv.ParentProduct!.Id == productId)
                : query.Where(pv => pv.ParentProduct!.Handle == request.ProductIdOrHandle);

            query = query
                .OrderBy(x => x.Id)
                .AsSplitQuery()
                .AsNoTracking()
                .AsQueryable();

            if (request.SearchString is not null)
            {
                query = query.Where(ca => ca.Name.ToLower().Contains(request.SearchString.ToLower()));
            }

            var totalCount = await query.CountAsync();

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection == API.SortDirection.Desc ? API.SortDirection.Desc : API.SortDirection.Desc);
            }

            var variants = await query
                .Include(pv => pv.ParentProduct)
                    .ThenInclude(pv => pv!.Category)
                .Include(pv => pv.ProductAttributes)
                    .ThenInclude(pv => pv!.Attribute)
                    .ThenInclude(pv => pv!.Values)
                .Include(pv => pv.ProductAttributes)
                    .ThenInclude(pv => pv!.Value)
                .Include(pv => pv.ProductOptions)
                    .ThenInclude(pv => pv.Option)
                    .ThenInclude(pv => pv.Group)
                .Include(pv => pv.ProductOptions)
                    .ThenInclude(pv => pv.Option)
                    .ThenInclude(pv => (pv as ChoiceOption)!.DefaultValue)
                .Include(pv => pv.ProductOptions)
                    .ThenInclude(pv => pv.Option)
                    .ThenInclude(pv => (pv as ChoiceOption)!.Values)
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync();

            return new PagedResult<ProductDto>(variants.Select(item => item.ToDto()), totalCount);
        }
        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}