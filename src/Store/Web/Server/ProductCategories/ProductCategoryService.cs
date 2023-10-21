namespace BlazorApp.ProductCategories;

public sealed class ProductCategoryService(CatalogAPI.IProductCategoriesClient productCategoriesClient) : IProductCategoryService
{
    public async Task<ProductCategoryTreeRootDto> GetProductCategories(CancellationToken cancellationToken = default)
    {
        var treeRoot = await productCategoriesClient.GetProductCategoryTreeAsync(cancellationToken);
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
    public static ProductCategoryDto ToDto(this CatalogAPI.ProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Description ??  string.Empty, productCategory.Handle, productCategory.Path, null, productCategory.ProductsCount);
    }

    public static ProductCategoryTreeNodeDto ToProductCategoryTreeNodeDto(this CatalogAPI.ProductCategoryTreeNode productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Description, productCategory.Parent?.ToParentDto(), productCategory.SubCategories.Select(x => x.ToProductCategoryTreeNodeDto()), productCategory.ProductsCount, productCategory.CanAddProducts);
    }

    public static ProductCategoryParent ToParentDto(this CatalogAPI.ParentProductCategoryTreeNode productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentDto(), productCategory.ProductsCount);
    }

    public static ProductCategoryParent ToParentDto2(this CatalogAPI.ParentProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentDto2(), productCategory.ProductsCount);
    }

     public static ProductCategoryParent ToParentDto3(this CatalogAPI.ProductCategory2 productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentDto2(), productCategory.ProductsCount);
    }
}