using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace YourBrand.Admin;

public sealed class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
        NavigationManager navigationManager)
        : base(provider, navigationManager)
    {
        ConfigureHandler(
            authorizedUrls: new[] { "https://localhost:5001", "https://yourbrand-admin-web.wonderfulsea-5402179d.swedencentral.azurecontainerapps.io" },
            scopes: new[] { "openid", "profile", "catalogapi" });
    }
}