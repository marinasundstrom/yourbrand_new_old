using YourBrand.Catalog.API.Persistence;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.API.Features.ProductManagement.Products;

public sealed record UpdateProductVisibility(string IdOrHandle, ProductVisibility Visibility) : IRequest<Result>
{
    public sealed class Handler(IPublishEndpoint publishEndpoint, CatalogContext catalogContext = default!) : IRequestHandler<UpdateProductVisibility, Result>
    {
        public async Task<Result> Handle(UpdateProductVisibility request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var product = isId ?
                await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Errors.ProductNotFound);
            }

            product.Visibility = (Domain.Enums.ProductVisibility)request.Visibility;

            await catalogContext.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new Catalog.Contracts.ProductVisibilityUpdated
            {
                ProductId = product.Id,
                Visibility = (Contracts.ProductVisibility)product.Visibility
            });

            return Result.Success();
        }
    }
}