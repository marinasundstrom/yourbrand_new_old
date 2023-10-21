# Secrets in Azure KeyVault

The following secrets should be in the KeyVault:

* ``yourbrand-catalog-svc-url`` - The URL of the catalog-svc container app 1) : ``https://yourbrand-catalog-svc``
* ``yourbrand-carts-svc-url`` - The URL of the carts-svc container app 1): ``https://yourbrand-carts-svc``
* ``yourbrand-catalog-db-connectionstring`` - Connection string with Azure Managed Identity
* ``yourbrand-carts-db-connectionstring`` - Connection string with Azure Managed Identity
* ``yourbrand-servicebus-connectionstring`` -  This one ``sb://yourbrand-servicebus.servicebus.windows.net``

## Notes

1) Using the URL used by service discovery.