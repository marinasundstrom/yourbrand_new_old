# Service Discovery

Services are discovered in a process that is called "service discovery". When a service starts up, it is registering with the service discovery service.

Our service discovery service is HashiCorp Consul. And we use Steeltoe simplify this registration process in code.

Consult is running in Docker.

## Adding service discovery

### Server

Add package reference:

```sh
dotnet add package Steeltoe.Discovery.Consul
```

Registering ``DiscoveryClient`` with dependency injection:

```csharp
using Steeltoe.Discovery.Client;

builder.Services.AddDiscoveryClient();
```

### Client

Register a client to a service using specified service name, like so:

```csharp
services.AddHttpClient("CatalogAPI", (sp, http) =>
{
    http.BaseAddress = new Uri("https://catalog-svc"); //Name in config, otherwise based on project name
});

services.AddHttpClient<IProductsClient>("CatalogAPI")
    .AddServiceDiscovery()
    .AddTypedClient<IProductsClient>((http, sp) => new CatalogAPI.ProductsClient(http));
```

If you need to use the ``AddServiceDiscovery()`` method from another class library:

```sh
dotnet add package Steeltoe.Common.Http"
```

## Configuration

In ``appsettings.json``:

```json
{
  "Consul": {
    "Discovery": {  
      "ServiceName": "catalog-svc",
      "Register": true
    }
  }
}
```

``Register`` default to ``true``, even when not specified.

In ``appsettings.Development.json``:

```json
{
    "Consul": {
        "Discovery": {  
            "PreferIpAddress": true
        }
    }
}
```

In ``appsettings.Production.json``:

```json
{
    "Consul": {
        "Discovery": {
            "Hostname": "localhost",
            "Scheme": "https" 
        }
    }
}
```