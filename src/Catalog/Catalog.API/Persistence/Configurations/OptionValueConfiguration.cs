using YourBrand.Catalog.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.Catalog.API.Persistence.Configurations;

public class OptionValueConfiguration : IEntityTypeConfiguration<OptionValue>
{
    public void Configure(EntityTypeBuilder<OptionValue> builder)
    {
        builder.ToTable("OptionValues");

        builder.Property(x => x.Price).HasColumnName(nameof(OptionValue.Price));
    }
}