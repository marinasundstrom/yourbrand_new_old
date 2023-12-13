using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace YourBrand.Admin;

public sealed class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
        NavigationManager navigationManager)
        : base(provider, navigationManager)
    {
#if DEBUG
        ConfigureHandler(
            authorizedUrls: new[] { "https://localhost:5001" },

        scopes: new[] { "openid", "profile", "catalogapi" });

#else
        ConfigureHandler(
            authorizedUrls: new[] { 
                "https://yourbrand-admin-web.wonderfulsea-5402179d.swedencentral.azurecontainerapps.io"            
            });
#endif
    }
}