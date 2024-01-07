namespace YourBrand.Catalog.API.Domain.Entities;

public class VatRate
{
    internal VatRate() { }

    public VatRate(string name, double factor, double factor2)
    {
        Name = name;
        Factor = factor;
        Factor2 = factor2;
    }

    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public double Rate { get; set; }

    /// <summary>
    /// Adds VAT (Total * Factor)
    /// </summary>
    /// <value></value>
    public double Factor { get; set; } = 1.25;

    /// <summary>
    /// Removes VAT (Total * Factor2)
    /// </summary>
    /// <value></value>
    public double Factor2 { get; set; } = 0.8;
}