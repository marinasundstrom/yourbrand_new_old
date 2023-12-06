using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products;

public sealed record CreateProduct(string Name, string Description, long CategoryId, bool IsGroupedProduct, decimal Price, string Handle) : IRequest<Result<ProductDto>>
{
    public sealed class Handler(IConfiguration configuration, CatalogContext catalogContext = default!) : IRequestHandler<CreateProduct, Result<ProductDto>>
    {
        public async Task<Result<ProductDto>> Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            var handleInUse = await catalogContext.Products.AnyAsync(product => product.Handle == request.Handle, cancellationToken);

            if (handleInUse)
            {
                return Result.Failure<ProductDto>(Errors.HandleAlreadyTaken);
            }

            var connectionString = catalogContext.Database.GetConnectionString()!;

            string cdnBaseUrl = (connectionString.Contains("localhost") || connectionString.Contains("mssql"))
                ? configuration["CdnBaseUrl"]!
                : "https://yourbrandstorage.blob.core.windows.net";

            var product = new Domain.Entities.Product()
            {
                Name = request.Name,
                Description = request.Description,
                Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
                HasVariants = request.IsGroupedProduct,
                Price = request.Price,
                Handle = request.Handle
            };

            var category = await catalogContext.ProductCategories
                .Include(x => x.Parent)
                .FirstAsync(x => x.Id == request.CategoryId, cancellationToken);

            category.AddProduct(product);

            catalogContext.Products.Add(product);

            await catalogContext.SaveChangesAsync(cancellationToken);

            return Result.Success(product.ToDto());
        }
    }
}