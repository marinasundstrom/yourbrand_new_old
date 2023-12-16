using Catalog.API.Persistence;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products;

public sealed record UpdateProductPrice(string IdOrHandle, decimal Price) : IRequest<Result>
{
    public sealed class Handler(IPublishEndpoint publishEndpoint, CatalogContext catalogContext = default!) : IRequestHandler<UpdateProductPrice, Result>
    {
        public async Task<Result> Handle(UpdateProductPrice request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var product = isId ?
                await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Errors.ProductNotFound);
            }

            if (product.RegularPrice is not null)
            {
                var p = product.RegularPrice.GetValueOrDefault();
                if (request.Price >= p)
                {
                    return Result.Failure(Errors.ProductPriceExceedsDiscountPrice);
                }
            }

            product.Price = request.Price;

            await catalogContext.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new Catalog.Contracts.ProductPriceUpdated
            {
                ProductId = product.Id,
                NewPrice = product.Price
            });

            return Result.Success();
        }
    }
}