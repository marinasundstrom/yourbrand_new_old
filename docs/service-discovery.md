# Service Discovery

Services are discovered in a process that is called "service discovery". When a service starts up, it is registering with the service discovery service.

Using this convention you can connect to a service:

```
https://<service-name>
```

The service name should be the same as the service in Azure.

Azure Container apps has built in Service Discovery support, using the same conventions as Steeltoe, and Consul.

## Load balancing

Load balancing is supported both by Azure Container apps, and through Consul.

## Running locally

Our local service discovery service is HashiCorp Consul. And we use Steeltoe simplify this registration process in code.

Consul is running in Docker.

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
    http.BaseAddress = new Uri("https://yourbrand-catalog-svc"); //Name in config, otherwise based on project name
})
.AddServiceDiscovery();

services.AddHttpClient<IProductsClient>("CatalogAPI")
    .AddTypedClient<IProductsClient>((http, sp) => new CatalogAPI.ProductsClient(http));
```

If you need to use the ``AddServiceDiscovery()`` method from another class library:

```sh
dotnet add package Steeltoe.Common.Http"
```

## Configuration

``Register`` default to ``true``, even when not specified.

In ``appsettings.Development.json``:

```json
{
    "Consul": {
        "Discovery": {
            "ServiceName": "yourbrand-catalog-svc",
            "Register": true,
            "Hostname": "localhost",
            "Scheme": "https" 
        }
    }
}
```