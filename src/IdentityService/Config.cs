using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
                // the api requires the role claim
                new ApiResource("catalogapi", "The Catalog API", new[] { JwtClaimTypes.Name, JwtClaimTypes.PreferredUserName, JwtClaimTypes.Email, JwtClaimTypes.Role })
                {
                    Scopes = new string[] { "catalogapi" }
                },
                // the api requires the role claim
                new ApiResource("cartsapi", "The Carts API", new[] { JwtClaimTypes.Name, JwtClaimTypes.PreferredUserName, JwtClaimTypes.Email, JwtClaimTypes.Role })
                {
                    Scopes = new string[] { "cartsapi" }
                }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalogapi", "Access the Catalog API"),
            new ApiScope("cartsapi", "Access the Carts API"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Duende.IdentityServer.Models.Client
            {
                ClientId = "clientapp",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                AllowedCorsOrigins = { "https://localhost:5001" },
                AllowedScopes = { "openid", "profile", "email", "catalogapi" },
                RedirectUris = { "https://localhost:5001/authentication/login-callback" },
                PostLogoutRedirectUris = { "https://localhost:5001/" },
                Enabled = true
            },
            new Duende.IdentityServer.Models.Client
            {
                ClientId = "storefront",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("foobar123!".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "profile", "email", "catalogapi", "cartsapi" },
            }
        };
}