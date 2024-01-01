using YourBrand.Catalog.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.Catalog.API.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder
            .Property(x => x.Handle)
            .HasMaxLength(150);

        builder.HasIndex(p => p.Handle);

        builder
            .HasMany(p => p.Options)
            .WithMany(p => p.Products)
            .UsingEntity<ProductOption>();

        builder
            .HasMany(x => x.OptionGroups)
            .WithOne(x => x.Product)
            .OnDelete(DeleteBehavior.ClientNoAction);

        /*
        builder
            .HasMany(p => p.Attributes)
            .WithMany(p => p.Products)
            .UsingEntity<ProductAttribute>();
        */

        builder
            .HasOne(x => x.ParentProduct)
            .WithMany(x => x.Variants)
            .HasForeignKey(x => x.ParentProductId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(x => x.ProductOptions)
            .WithOne(x => x.Product)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(p => p.Category)
            .WithMany(p => p.Products);
    }
}
