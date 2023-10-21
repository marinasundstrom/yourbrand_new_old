using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.ProductCategories;

public sealed record UpdateProductCategoryDetails(string IdOrPath, string Name, string Description) : IRequest<Result>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<UpdateProductCategoryDetails, Result>
    {
        public async Task<Result> Handle(UpdateProductCategoryDetails request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrPath, out var id);

            var product = isId ?
                await catalogContext.ProductCategories.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.ProductCategories.FirstOrDefaultAsync(product => product.Path == request.IdOrPath, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Errors.ProductCategoryNotFound);
            }

            product.Name = request.Name;
            product.Description = request.Description;

            await catalogContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}