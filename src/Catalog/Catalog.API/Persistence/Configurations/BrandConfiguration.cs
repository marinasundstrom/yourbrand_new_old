using Catalog.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Persistence.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands");

        builder.HasIndex(x => x.Handle);
    }
}

/*
public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
{
    public void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.ToTable("Merchants");
    }
}
*/
