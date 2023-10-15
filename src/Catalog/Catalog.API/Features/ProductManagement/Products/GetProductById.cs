using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products;

public sealed record GetProductById(string IdOrHandle) : IRequest<Result<ProductDto>>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<GetProductById, Result<ProductDto>>
    {
        public async Task<Result<ProductDto>> Handle(GetProductById request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var query = catalogContext.Products
                .Include(x => x.Category)
                .ThenInclude(x => x.Parent)
                .AsQueryable();

            var product = isId ?
                await query.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await query.FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            return product is null
                ? Result.Failure<ProductDto>(Errors.ProductNotFound)
                : Result.Success(product.ToDto());
        }
    }
}