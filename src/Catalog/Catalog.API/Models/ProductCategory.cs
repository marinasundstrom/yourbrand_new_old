namespace Catalog.API.Model;

public sealed class ProductCategory 
{
    private HashSet<Product> _products = new HashSet<Product>();
    private HashSet<ProductCategory> _subCategories = new HashSet<ProductCategory>();

    public long Id { get; private set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public ProductCategory? Parent { get; set; }

    public bool CanAddProducts { get; set; }

    public IReadOnlyCollection<Product> Products => _products;

    public long ProductsCount { get; set; }

    public void AddProduct(Product product) 
    {
        if(!CanAddProducts)
        {
            throw new InvalidOperationException("Can not add products.");
        }

        product.Category = this;
        _products.Add(product);

        IncrementProductsCount();
    }

    public void RemoveProduct(Product product) 
    {
        product.Category = null;
        _products.Remove(product);
        
        DecrementProductsCount();
    }

    public IReadOnlyCollection<ProductCategory> SubCategories => _subCategories;

    public void AddSubCategory(ProductCategory productCategory) 
    {
        productCategory.Parent = this;
        productCategory.Path = $"{(Path is null ? Handle : Path)}/{productCategory.Handle}";
        _subCategories.Add(productCategory);
    }

    public void RemoveSubCategory(ProductCategory productCategory) 
    {
        productCategory.Parent = null;
        _subCategories.Remove(productCategory);
    }

    private void IncrementProductsCount()
    {
        ProductsCount++;

        Parent?.IncrementProductsCount();
    }

    private void DecrementProductsCount()
    {
        ProductsCount--;

        Parent?.DecrementProductsCount();
    }

    public string Handle { get; set; } = default!;

    public string Path { get; set; } = default!;
}