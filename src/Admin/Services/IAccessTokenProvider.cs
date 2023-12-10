namespace YourBrand.Admin.Services;

public interface IAccessTokenProvider
{
    Task<string?> GetAccessTokenAsync();
}

