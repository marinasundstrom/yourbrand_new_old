using Blazored.LocalStorage;

using YourBrand.Catalog;

namespace YourBrand.Client;

public interface IStoreProvider
{
    Task<IEnumerable<Store>> GetAvailableStoresAsync();

    Store? CurrentStore { get; set; }

    Task SetCurrentStore(string storeId);

    event EventHandler? CurrentStoreChanged;
}

public sealed class StoreProvider : IStoreProvider
{
    IStoresClient _storesClient;
    private readonly ILocalStorageService _localStorageService;
    IEnumerable<Store> _stores;

    public StoreProvider(IStoresClient storesClient, ILocalStorageService localStorageService)
    {
        _storesClient = storesClient;
        _localStorageService = localStorageService;
    }

    public async Task<IEnumerable<Store>> GetAvailableStoresAsync()
    {
        var items = _stores = (await _storesClient.GetStoresAsync(0, null, null, null, null)).Items;
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