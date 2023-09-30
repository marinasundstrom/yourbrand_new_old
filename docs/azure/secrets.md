# Secrets in Azure KeyVault

The following secrets should be in the KeyVault:

* ``yourbrand-catalog-api-url`` - The URL
* ``yourbrand-carts-api-url`` - The URL
* ``yourbrand-catalog-db-connectionstring`` - Connection string with Azure Managed Identity
* ``yourbrand-carts-db-connectionstring`` - Connection string with Azure Managed Identity
* ``yourbrand-servicebus-connectionstring`` - format ``sb://your-service-bus-namespace.servicebus.windows.net``