﻿using YourBrand.Catalog.API.Common;
using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.API.Features.Brands.Queries;

public sealed record GetBrandQuery(int Id) : IRequest<BrandDto?>
{
    sealed class GetBrandQueryHandler : IRequestHandler<GetBrandQuery, BrandDto?>
    {
        private readonly CatalogContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetBrandQueryHandler(
            CatalogContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<BrandDto?> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            var brand = await _context
               .Brands
               .AsNoTracking()
               .FirstAsync(c => c.Id == request.Id);

            if (brand is null)
            {
                return null;
            }

            return brand.ToDto();
        }
    }
}