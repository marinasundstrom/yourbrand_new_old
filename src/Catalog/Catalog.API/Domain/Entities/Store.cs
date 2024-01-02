namespace YourBrand.Catalog.API.Domain.Entities;

public class Store : Entity<string>
{
    protected Store() { }

    public Store(string name, string handle, Currency currency) : base(Guid.NewGuid().ToString())
    {
        Name = name;
        Handle = handle;
        Currency = currency;
    }

    public string Name { get; set; } = null!;

    public string Handle { get; set; } = null!;

    public Currency Currency { get; set; } = null!;

    public CurrencyDisplayOptions CurrencyDisplayOptions { get; set; }

    public PricingOptions PricingOptions { get; set; }

    public List<Product> Products { get; } = new List<Product>();
}

public class CurrencyDisplayOptions
{
    public bool IncludeVatInSalesPrice { get; set; } = false;
    public int? RoundingDecimals { get; set; } = null;
}

public class PricingOptions
{
    public double ProfitMarginPercentage { get; set; } = 0.2;

    public List<CategoryPricingOptions> CategoryPricingOptions { get; set; } = new List<CategoryPricingOptions>();
}

public class CategoryPricingOptions
{
    public string CategoryId { get; set; } = default!;
    public double ProfitMarginRate { get; set; } = 0.2;
}
