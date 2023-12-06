# Resources in Azure

## Container Registry

A container registry.

Enable Admin User, for listing images in the portal.

## App Configuration

Named: ``yourbrand-appconfiguration``

## KeyVault

A KeyVault named: ``yourbrand-keyvault``

## Container apps

Create a container app environment with containers:

* ``yourbrand-store-web``
* ``yourbrand-catalog-svc``
* ``yourbrand-carts-svc``
* ``yourbrand-storefront-svc``

Make sure that the container of each container app has a system-assigned managed identity.

More on that below.

## SQL Databases

Provided that you have a SQL server.

Create a SQL Server and the following database:

* ``yourbrand-catalog-db``
* ``yourbrand-carts-db``
* ``yourbrand-storefront-db``

Enable password for connection string used by migrations.

## Azure Service Bus

Name it ``yourbrand-servicebus``. 

Choose Pricing Tier "Standard" or above.

## Storage Account

``yourbrandstorage`` _(dashes are not allowed in name)_