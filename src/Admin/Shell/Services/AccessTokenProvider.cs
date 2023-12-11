using System;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

using YourBrand.Admin.Services;

namespace YourBrand.Admin.Services;

public class AccessTokenProvider : YourBrand.Admin.Services.IAccessTokenProvider
{
    private readonly Microsoft.AspNetCore.Components.WebAssembly.Authentication.IAccessTokenProvider _accessTokenProvider;

    public AccessTokenProvider(Microsoft.AspNetCore.Components.WebAssembly.Authentication.IAccessTokenProvider accessTokenProvider)
    {
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        var results = await _accessTokenProvider.RequestAccessToken(new AccessTokenRequestOptions() { Scopes = new[] { "myapi" } });

        if (results.TryGetToken(out var accessToken))
        {
            return accessToken.Value;
        }

        return null;
    }
}