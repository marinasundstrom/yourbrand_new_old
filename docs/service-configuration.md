# Service configuration

Each service can be configured using key-value pairs that come from different sources.

The sources that configuration settings get loaded from are the following, in the order of precedence:

1. The ``appsettings.json`` file
2. Any ``appsettings.{ ENVIRONMENT_NAME }.json`` files
3. User secrets (from the .NET User Secrets Manager)
4. Environment variables
5. Azure Key Vault
6. Azure App Configuration
7. Command line arguments

This means that you can override configuration.

In the services, configuration will be available through the ``Configuration`` property on the apps builder and host objects, or via the injectable ``IConfiguration`` object.

## Conventions

The configuration can be visualized as a hierarchy in JSON:

```json
{
    "YourBrand": {
        "catalog-svc": {
            "Url": "http://catalog-svc/"
        }
    }
}
```

This is the format used in ``appsettings.json``.

### Keys and their forms

Configuration keys are case-insensitive.

| Key used in-app and by  App Configuration     | Key in Azure KeyVault                      | Environment variable                       |
|-----------------------------------------------|--------------------------------------------|--------------------------------------------|
| ``yourbrand:catalog-svc:url``                 | ``yourbrand--catalog-svc--url``            | ``yourbrand__catalog-svc__url``            |


The ``--`` (double dash) corresponds to a new level in the hierarchy. In in-app configuration the ``:`` is used instead. For environment variables we use ``__`` (double underscore)


## Adding or changing configuration

In Azure, you can add a setting to either App Configuration, or KeyVault.

In a service that is running locally, you can change the ``appsettings.Development.json`` file, or set environment variables in ``Properties/launchSettings.json``.

In Docker, you can set the environment variables. Also look at the docker-compose files.