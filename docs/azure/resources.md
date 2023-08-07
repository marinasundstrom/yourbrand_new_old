# Resources in Azure

## Container Registry

A container registry.

## KeyVault

A KeyVault named: ``yourbrand-keyvault``

## Container apps

Create a container app environment with containers:

* ``yourbrand-store-web``
* ``yourbrand-catalog-api``
* ``yourbrand-carts-api``

Make sure that the container of each container app has a system-assigned managed identity.

More on that below.

## SQL Databases

Provided that you have a SQL server.

Create a SQL Server and the following database:

* ``yourbrand-catalog-db``
* ``yourbrand-carts-db``

## Azure Service Bus

Name it ``yourbrand-servicebus``. 

Choose Pricing Tier "Standard" or above.