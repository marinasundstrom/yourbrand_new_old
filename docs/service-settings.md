# Service settings

Each service can be configured using settings (key-value pairs) that come from these sources.

* Environment variables
* appsettings.json (environment specific)
* Azure App Configuration
* Azure KeyVault

In the services, settings will be available through app host object, or via the injectable ``IConfiguration`` object.

## Conventions

Settings can be visualized as an hierarchy:

```json
{
    "YourBrand": {
        "catalog-svc": {
            "Url": "http://catalog-svc/"
        }
    }
}
```

### Keys and names

| Key (used in-app to retrieve)    | Key in Azure KeyVault & App Configuration  |
|----------------------------------|--------------------------------------------|
| ``yourbrand:catalog-svc:url``    | ``yourbrand--catalog-svc--url``            |


Double dash ``--`` corresponds to a new level in the hierarchy. In in-app configuration the ``:`` is used instead.


## Adding or changing settings

In Azure, you can add a setting to either App Configuration, or KeyVault.

In a service that is running locally, you can change the ``appsettings.Development.json`` file, or set environment variables in ``Properties/launchSettings.json``.

In Docker, you can set the environment variables. Also look at the docker-compose files.