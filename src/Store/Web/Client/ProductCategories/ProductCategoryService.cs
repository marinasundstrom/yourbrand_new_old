namespace BlazorApp.ProductCategories;

public sealed class ProductCategoryService(StoreWeb.IProductCategoriesClient productCategoriesClient) : IProductCategoryService
{
    public async Task<ProductCategoryTreeRootDto> GetProductCategories(CancellationToken cancellationToken = default)
    {
        var treeRoot = await productCategoriesClient.GetProductCategoriesAsync(cancellationToken);
        return new ProductCategoryTreeRootDto(treeRoot.Categories.Select(x => x.ToProductCategoryTreeNodeDto()), treeRoot.ProductsCount);
    }

    public async Task<ProductCategoryDto> GetProductCategoryById(string productCategoryId, CancellationToken cancellationToken = default)
    {
        var productCategory = await productCategoriesClient.GetProductCategoryByIdAsync(productCategoryId, cancellationToken);
        return productCategory.ToDto();
    }
}

public static class Mapping
{
    public static ProductCategoryDto ToDto(this StoreWeb.ProductCategoryDto productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Description, productCategory.Handle, productCategory.Path, null, productCategory.ProductsCount);
    }

    public static ProductCategoryParent ToParentDto(this StoreWeb.ProductCategoryParent productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentDto(), productCategory.ProductsCount);
    }

    public static ProductCategoryTreeNodeDto ToProductCategoryTreeNodeDto(this StoreWeb.ProductCategoryTreeNodeDto productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Description, productCategory.Parent?.ToParentDto(), productCategory.SubCategories.Select(x => x.ToProductCategoryTreeNodeDto()), productCategory.ProductsCount, productCategory.CanAddProducts);
    }
}