# Secrets in Azure KeyVault

The following secrets should be in the KeyVault:

* ``yourbrand-catalog-api-url`` - The URL of the catalog-api container app
* ``yourbrand-carts-api-url`` - The URL of the carts-api container app
* ``yourbrand-catalog-db-connectionstring`` - Connection string with Azure Managed Identity
* ``yourbrand-carts-db-connectionstring`` - Connection string with Azure Managed Identity
* ``yourbrand-servicebus-connectionstring`` -  This one ``sb://yourbrand-servicebus.servicebus.windows.net``