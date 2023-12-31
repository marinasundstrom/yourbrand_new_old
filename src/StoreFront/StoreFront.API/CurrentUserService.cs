﻿using System.Net;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using Polly;

namespace YourBrand.StoreFront.API;

public sealed class CurrentUserService : ICurrentUserService
{
    private string? _currentUserId;
    private HttpContext httpContext;
    private string? host;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor.HttpContext!;
    }

    public string? UserId => _currentUserId ??= httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? ClientId => httpContext.Request.Headers["X-Client-Id"].FirstOrDefault();

    public string? SessionId => httpContext?.Request.Headers["X-Session-Id"].FirstOrDefault();

    public int? CustomerNo
    {
        get
        {
            var str = httpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "CustomerId")?.Value;

            if (str is null) return null;

            return int.Parse(str);
        }
    }

    public string? Host
    {
        get
        {
            var parts = httpContext?.Request.Host.Host.Split('.');
            if (parts!.Count() > 2)
            {
                return host ??= parts!.First();
            }
            return null;
        }
    }

    public string? UserAgent => httpContext?.Request.Headers.UserAgent.ToString();

    public string? GetRemoteIPAddress(bool allowForwarded = true)
    {
        if (allowForwarded)
        {
            string header = (httpContext.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault())!;
            if (IPAddress.TryParse(header, out IPAddress? ip))
            {
                return ip.ToString();
            }
        }
        return httpContext.Connection.RemoteIpAddress?.ToString();
    }
}