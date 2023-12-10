using Catalog.API.Domain.Entities;
using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products;

public sealed record DeleteProduct(string IdOrHandle) : IRequest<Result>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<DeleteProduct, Result>
    {
        public async Task<Result> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var product = isId ?
                await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Errors.ProductNotFound);
            }

            await catalogContext.ProductAttributes
                .Where(x => x.ProductId == product.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await catalogContext.AttributeGroups
                .Where(x => x.Product!.Id == product.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await catalogContext.ProductOptions
                .Where(x => x.ProductId == product.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await catalogContext.Products
                .Where(x => x.ParentProductId == product.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await catalogContext.ProductVariantOptions
                .Where(x => x.ProductId == product.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await catalogContext.Options
                .Where(x => x.Group!.Product!.Id == product.Id)
                .ExecuteUpdateAsync(x => x.SetProperty(z => ((ChoiceOption)z).DefaultValue, (OptionValue?)null), cancellationToken);

            await catalogContext.OptionValues
                .Where(x => x.Option.Group!.Product!.Id == product.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await catalogContext.Options
                .Where(x => x.Group!.Product!.Id == product.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await catalogContext.OptionGroups
                .Where(x => x.Product!.Id == product.Id)
                .ExecuteDeleteAsync(cancellationToken);

            catalogContext.Products.Remove(product);

            await catalogContext.SaveChangesAsync(cancellationToken);


            return Result.Success();
        }
    }
}