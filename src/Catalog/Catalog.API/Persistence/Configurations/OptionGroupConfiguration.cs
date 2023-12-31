﻿using YourBrand.Catalog.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.Catalog.API.Persistence.Configurations;

public class OptionGroupConfiguration : IEntityTypeConfiguration<OptionGroup>
{
    public void Configure(EntityTypeBuilder<OptionGroup> builder)
    {
        builder.ToTable("OptionGroups");

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.OptionGroups)
            .OnDelete(DeleteBehavior.ClientNoAction);
    }
}