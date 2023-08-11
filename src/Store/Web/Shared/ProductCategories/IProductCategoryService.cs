namespace BlazorApp.ProductCategories;

public interface IProductCategoryService
{
    Task<ProductCategoryTreeRootDto> GetProductCategories(CancellationToken cancellationToken = default);

    Task<ProductCategoryDto> GetProductCategoryById(string productCategoryId, CancellationToken cancellationToken = default);
}

public record class ProductCategoryDto(long Id, string Name, string Description, string Handle, string Path, ParentProductCategoryDto? Parent, long ProductsCount);

public record class ProductCategoryTreeRootDto(IEnumerable<ProductCategoryTreeNodeDto> Categories, long ProductsCount);

public record class ProductCategoryTreeNodeDto(long Id, string Name, string Handle, string Path, string? Description, ParentProductCategoryDto? Parent, IEnumerable<ProductCategoryTreeNodeDto> SubCategories, long ProductsCount, bool CanAddProducts);

public record class ParentProductCategoryDto(long Id, string Name, string Handle, string Path, ParentProductCategoryDto? Parent, long ProductsCount);