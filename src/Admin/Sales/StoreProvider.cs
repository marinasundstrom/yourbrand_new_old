using Blazored.LocalStorage;

using YourBrand.Admin.Services;
using YourBrand.Catalog;

using Store = YourBrand.Admin.Services.Store;

namespace YourBrand.Sales;

public sealed class StoreProvider : IStoreProvider
{
    readonly IStoresClient _storesClient;
    private readonly ILocalStorageService _localStorageService;
    IEnumerable<Store> _stores;

    public StoreProvider(IStoresClient storesClient, ILocalStorageService localStorageService)
    {
        _storesClient = storesClient;
        _localStorageService = localStorageService;
    }

    public async Task<IEnumerable<Store>> GetAvailableStoresAsync()
    {
        var items = _stores = (await _storesClient.GetStoresAsync(0, null, null, null, null)).Items
            .Select(x => new Store(x.Id, x.Name, x.Handle, new Admin.Services.Currency(x.Currency.Code, x.Currency.Name, x.Currency.Symbol)));

        if (CurrentStore is null)
        {
            var storeId = await _localStorageService.GetItemAsStringAsync("storeId");
            await SetCurrentStore(storeId ?? items.First().Id);
        }
        return items;
    }

    public Store? CurrentStore { get; set; }

    public async Task SetCurrentStore(string storeId)
    {
        if (_stores is null)
        {
            await GetAvailableStoresAsync();
        }

        CurrentStore = _stores!.FirstOrDefault(x => x.Id == storeId);

        await _localStorageService.SetItemAsStringAsync("storeId", storeId);

        CurrentStoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? CurrentStoreChanged;
}