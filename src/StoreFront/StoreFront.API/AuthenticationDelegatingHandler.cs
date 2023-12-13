using System.Net;
using System.Net.Http.Headers;

using Azure.Core;

using IdentityModel.Client;

using Microsoft.Identity.Client;

public class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationDelegatingHandler> _logger;

    public AuthenticationDelegatingHandler(IConfiguration configuration, ILogger<AuthenticationDelegatingHandler> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await GetCachedOrRequestNewTokenAsync();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
        {
            accessToken = await RequestTokenAsync();

            _logger.LogInformation("Retrieved new access token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            response = await base.SendAsync(request, cancellationToken);
        }

        return response;
    }

    private static string? accessToken;

    async Task<string?> GetCachedOrRequestNewTokenAsync()
    {
        if (accessToken is null)
        {
            accessToken = await RequestTokenAsync();

            _logger.LogInformation("Retrieved new access token");

            return accessToken;

        }

        _logger.LogInformation("Retrieved cached access token");

        return accessToken;
    }

    async Task<string?> RequestTokenAsync()
    {
        var isDev = _configuration["ASPNETCORE_ENVIRONMENT"] == "Development";

        if (isDev)
        {
            return await Local(_configuration);
        }

        return await Production(_configuration);
    }

    async Task<string?> Production(IConfiguration configuration)
    {
        IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(configuration.GetValue<string>("AzureAd:ClientId"))
               .WithTenantId(configuration.GetValue<string>("AzureAd:TenantId"))
               .WithClientSecret(configuration.GetValue<string>("AzureAd:StoreFront:ClientCredentials:ClientSecret"))
               .Build();

        var c = app.AcquireTokenForClient(configuration.GetSection("AzureAd:StoreFront:Scopes").Get<List<string>>());
        var x = await c.ExecuteAsync();
        return x.AccessToken;
    }

    async Task<string?> Local(IConfiguration configuration)
    {
        // discover endpoints from metadata
        var client = new HttpClient();

        var disco = await client.GetDiscoveryDocumentAsync(_configuration.GetValue<string>("StoreFront:Authority"));
        if (disco.IsError)
        {
            Console.WriteLine(disco.Error);
            throw new Exception();
        }

        // request token
        TokenResponse tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,

            ClientId = configuration.GetValue<string>("StoreFront:ClientCredentials:ClientId")!,
            ClientSecret = configuration.GetValue<string>("StoreFront:ClientCredentials:ClientSecret"),
            Scope = configuration.GetValue<string>("StoreFront:Scope"),
        });
        if (tokenResponse.IsError)
        {
            Console.WriteLine(tokenResponse.Error);
        }

        Console.WriteLine(tokenResponse.Json);

        return tokenResponse.AccessToken;
    }
}