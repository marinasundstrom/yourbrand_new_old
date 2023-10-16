using Catalog.API.Features.Currencies;

namespace Catalog.API.Features.Stores;

public record class StoreDto(string Id, string Name, string Handle, CurrencyDto Currency);