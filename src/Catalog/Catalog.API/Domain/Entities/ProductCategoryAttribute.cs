namespace Catalog.API.Domain.Entities;

public class ProductCategoryAttribute : Entity<Guid>
{
    public long ProductCategoryId { get; set; }

    public ProductCategory ProductCategory { get; set; } = null!;

    //public ProductCategoryAttribute InheritedFromId { get; set; } = null!;

    //public ProductCategoryAttribute InheritedFrom { get; set; } = null!;

    public string AttributeId { get; set; } = null!;

    public Attribute Attribute { get; set; } = null!;
}