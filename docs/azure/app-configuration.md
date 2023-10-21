# App Configuration

The following settings should be in ``yourbrand-appconfiguration``:

* ``yourbrand--catalog-svc--url`` - The URL of the catalog-svc container app 1) : ``https://yourbrand-catalog-svc``
* ``yourbrand--carts-svc--url`` - The URL of the carts-svc container app 1): ``https://yourbrand-carts-svc``
* ``yourbrand--catalog-svc--db--connectionstring`` - Connection string with Azure Managed Identity
* ``yourbrand--carts-svc--db--connectionstring`` - Connection string with Azure Managed Identity

```json
{
    "yourbrand": {
        "catalog-svc": {
            "url": "",
            "db": {
                "connectionstring": ""
            }
        },
    },
    "Azure": {
        "ServiceBus": {
            "Namespace": "yourbrand-servicebus"
        }
    }
}
```

## In-project service settings for Azure

Each service has its Azure dependencies specified in their ``appsettings.Production.json`` file.

There you can see what ``AppConfig``, ``KeyVault``, or ``StorageAccount`` they use.

## Notes

1) Using the URL used by service discovery.