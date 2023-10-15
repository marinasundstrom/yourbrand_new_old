﻿// <auto-generated />
using System;
using Catalog.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Catalog.API.Migrations
{
    [DbContext(typeof(CatalogContext))]
    [Migration("20231015181315_AddEntities")]
    partial class AddEntities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.2.23480.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Catalog.API.Domain.Entities.Attribute", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ProductCategoryId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("Attributes", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.AttributeGroup", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<int?>("Seq")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("AttributeGroups", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.AttributeValue", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AttributeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Seq")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.ToTable("AttributeValues", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Handle")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Handle");

                    b.ToTable("Brands", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Continent", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.ToTable("Continents", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Country", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Capital")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContinentCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NativeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.HasIndex("ContinentCode");

                    b.ToTable("Countries", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.CountryCurrency", b =>
                {
                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CountryCode", "CurrencyCode");

                    b.HasIndex("CurrencyCode");

                    b.ToTable("CountryCurrencies", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.CountryLanguage", b =>
                {
                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LanguageCode")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CountryCode", "LanguageCode");

                    b.HasIndex("LanguageCode");

                    b.ToTable("CountryLanguages", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Currency", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.ToTable("Currencies", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Language", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NativeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.ToTable("Languages", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Option", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OptionType")
                        .HasColumnType("int");

                    b.Property<long?>("ProductCategoryId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("Options", (string)null);

                    b.HasDiscriminator<int>("OptionType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.OptionGroup", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Max")
                        .HasColumnType("int");

                    b.Property<int?>("Min")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<int?>("Seq")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("OptionGroups", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.OptionValue", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OptionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SKU")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("InventoryProductId");

                    b.Property<int?>("Seq")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OptionId");

                    b.ToTable("OptionValues", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Handle")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("HasVariants")
                        .HasColumnType("bit");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsCustomizable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ParentProductId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("RegularPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("StoreId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Visibility")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Handle");

                    b.HasIndex("ParentProductId");

                    b.HasIndex("StoreId");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ProductAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AttributeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("ForVariant")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMainAttribute")
                        .HasColumnType("bit");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<string>("ValueId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ValueId");

                    b.ToTable("ProductAttributes", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ProductCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("CanAddProducts")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Handle")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<long>("ProductsCount")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Handle");

                    b.HasIndex("ParentId");

                    b.HasIndex("Path");

                    b.ToTable("ProductCategories", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ProductCategoryAttribute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AttributeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("ProductCategoryId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("ProductCategoryAttributes", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ProductOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("IsInherited")
                        .HasColumnType("bit");

                    b.Property<string>("OptionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OptionId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductOptions", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ProductVariantOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("IsSelected")
                        .HasColumnType("bit");

                    b.Property<string>("OptionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<string>("ProductVariantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ProductVariantId1")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OptionId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProductVariantId1");

                    b.ToTable("ProductVariantOption", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode");

                    b.ToTable("Regions", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Store", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Handle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyCode");

                    b.ToTable("Stores", (string)null);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ChoiceOption", b =>
                {
                    b.HasBaseType("Catalog.API.Domain.Entities.Option");

                    b.Property<string>("DefaultValueId")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("DefaultValueId");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.NumericalValueOption", b =>
                {
                    b.HasBaseType("Catalog.API.Domain.Entities.Option");

                    b.Property<int?>("DefaultNumericalValue")
                        .HasColumnType("int");

                    b.Property<int?>("MaxNumericalValue")
                        .HasColumnType("int");

                    b.Property<int?>("MinNumericalValue")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.SelectableOption", b =>
                {
                    b.HasBaseType("Catalog.API.Domain.Entities.Option");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("bit");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SKU")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("InventoryProductId");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.TextValueOption", b =>
                {
                    b.HasBaseType("Catalog.API.Domain.Entities.Option");

                    b.Property<string>("DefaultTextValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TextValueMaxLength")
                        .HasColumnType("int");

                    b.Property<int?>("TextValueMinLength")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Attribute", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.AttributeGroup", "Group")
                        .WithMany("Attributes")
                        .HasForeignKey("GroupId");

                    b.HasOne("Catalog.API.Domain.Entities.ProductCategory", "ProductCategory")
                        .WithMany("Attributes")
                        .HasForeignKey("ProductCategoryId");

                    b.Navigation("Group");

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.AttributeGroup", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Product", "Product")
                        .WithMany("AttributeGroups")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.AttributeValue", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Attribute", "Attribute")
                        .WithMany("Values")
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Country", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Continent", "Continent")
                        .WithMany()
                        .HasForeignKey("ContinentCode");

                    b.Navigation("Continent");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.CountryCurrency", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Country", "Country")
                        .WithMany("CountryCurrencies")
                        .HasForeignKey("CountryCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.API.Domain.Entities.Currency", "Currency")
                        .WithMany("CountryCurrencies")
                        .HasForeignKey("CurrencyCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.CountryLanguage", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Country", "Country")
                        .WithMany("CountryLanguages")
                        .HasForeignKey("CountryCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.API.Domain.Entities.Language", "Language")
                        .WithMany("CountryLanguages")
                        .HasForeignKey("LanguageCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Language");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Option", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.OptionGroup", "Group")
                        .WithMany("Options")
                        .HasForeignKey("GroupId");

                    b.HasOne("Catalog.API.Domain.Entities.ProductCategory", "ProductCategory")
                        .WithMany("Options")
                        .HasForeignKey("ProductCategoryId");

                    b.Navigation("Group");

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.OptionGroup", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Product", "Product")
                        .WithMany("OptionGroups")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.OptionValue", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.ChoiceOption", "Option")
                        .WithMany("Values")
                        .HasForeignKey("OptionId");

                    b.Navigation("Option");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Product", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.ProductCategory", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.HasOne("Catalog.API.Domain.Entities.Product", "ParentProduct")
                        .WithMany("Variants")
                        .HasForeignKey("ParentProductId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Catalog.API.Domain.Entities.Store", null)
                        .WithMany("Products")
                        .HasForeignKey("StoreId");

                    b.Navigation("Category");

                    b.Navigation("ParentProduct");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ProductAttribute", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Attribute", "Attribute")
                        .WithMany("ProductAttributes")
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Catalog.API.Domain.Entities.Product", "Product")
                        .WithMany("ProductAttributes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.API.Domain.Entities.AttributeValue", "Value")
                        .WithMany("ProductAttributes")
                        .HasForeignKey("ValueId");

                    b.Navigation("Attribute");

                    b.Navigation("Product");

                    b.Navigation("Value");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ProductCategory", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.ProductCategory", "Parent")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ProductCategoryAttribute", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Attribute", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.API.Domain.Entities.ProductCategory", "ProductCategory")
                        .WithMany()
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ProductOption", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Option", "Option")
                        .WithMany()
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.API.Domain.Entities.Product", "Product")
                        .WithMany("ProductOptions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Option");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ProductVariantOption", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Option", "Option")
                        .WithMany("ProductVariantOptions")
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Catalog.API.Domain.Entities.Product", "Product")
                        .WithMany("ProductVariantOptions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Catalog.API.Domain.Entities.Product", "ProductVariant")
                        .WithMany()
                        .HasForeignKey("ProductVariantId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Option");

                    b.Navigation("Product");

                    b.Navigation("ProductVariant");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Region", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Country", "Country")
                        .WithMany("Regions")
                        .HasForeignKey("CountryCode");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Store", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ChoiceOption", b =>
                {
                    b.HasOne("Catalog.API.Domain.Entities.OptionValue", "DefaultValue")
                        .WithMany()
                        .HasForeignKey("DefaultValueId");

                    b.Navigation("DefaultValue");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Attribute", b =>
                {
                    b.Navigation("ProductAttributes");

                    b.Navigation("Values");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.AttributeGroup", b =>
                {
                    b.Navigation("Attributes");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.AttributeValue", b =>
                {
                    b.Navigation("ProductAttributes");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Country", b =>
                {
                    b.Navigation("CountryCurrencies");

                    b.Navigation("CountryLanguages");

                    b.Navigation("Regions");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Currency", b =>
                {
                    b.Navigation("CountryCurrencies");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Language", b =>
                {
                    b.Navigation("CountryLanguages");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Option", b =>
                {
                    b.Navigation("ProductVariantOptions");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.OptionGroup", b =>
                {
                    b.Navigation("Options");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Product", b =>
                {
                    b.Navigation("AttributeGroups");

                    b.Navigation("OptionGroups");

                    b.Navigation("ProductAttributes");

                    b.Navigation("ProductOptions");

                    b.Navigation("ProductVariantOptions");

                    b.Navigation("Variants");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ProductCategory", b =>
                {
                    b.Navigation("Attributes");

                    b.Navigation("Options");

                    b.Navigation("Products");

                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.Store", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Catalog.API.Domain.Entities.ChoiceOption", b =>
                {
                    b.Navigation("Values");
                });
#pragma warning restore 612, 618
        }
    }
}
