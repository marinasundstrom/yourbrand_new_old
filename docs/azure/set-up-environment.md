# Set up Azure environment

This can be done from the portal:

## Install Azure CLI

Make sure that Azure CLI has been installed.

Then log in:

```
az login
```

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

Create a SQL Server and the following database:

* ``yourbrand-catalog-db``
* ``yourbrand-carts-db``

Add users for each container app to each DB:

In ``yourbrand-catalog-db`` run this:

```
CREATE USER [yourbrand-catalog-api] FROM EXTERNAL PROVIDER
ALTER ROLE db_datareader ADD MEMBER [yourbrand-catalog-api]
ALTER ROLE db_datawriter ADD MEMBER [yourbrand-catalog-api]
```

And, in ``yourbrand-carts-db`` run this:

```
CREATE USER [yourbrand-carts-api] FROM EXTERNAL PROVIDER
ALTER ROLE db_datareader ADD MEMBER [yourbrand-carts-api]
ALTER ROLE db_datawriter ADD MEMBER [yourbrand-carts-api]
```

## Create Service Bus

Name it ``yourbrand-servicebus``. 

Choose Pricing Tier "Standard" or above.

## Assigning managed identities

Make sure to enable Managed Identity  for each resource. 

This project uses System-assigned managed identities.

Setting up managed identities, and the permissions for resources accessing other resources.

Key Vault access:

*  Both ``yourbrand-catalog-api``, ``yourbrand-carts-api``,and ``yourbrand-store-web`` should be 
"Key Vault Secrets User" to ``yourbrand-keyvault``

Database access:

* ``yourbrand-catalog-api`` should have read write access to ``yourbrand-catalog-db``
* ``yourbrand-carts-api`` should have read write access to ``yourbrand-carts-db``

Service bus access:

Both ``yourbrand-catalog-api``, ``yourbrand-carts-api``, and ``yourbrand-store-web`` should have these roles to ``yourbrand-servicebus``:

* Azure Service Bus Data Owner
* Azure Service Bus Data Receiver
* Azure Service Bus Data Sender

## Add secrets to KeyVault

Add these secrets with values

* ``yourbrand-catalog-api-url``
* ``yourbrand-carts-api-url``
* ``yourbrand-catalog-db-connectionstring``
* ``yourbrand-carts-db-connectionstring``
* ``yourbrand-servicebus-connectionstring``

Container registry:

And, the container apps need permissions to pull images from the container registry.

## Scale rule for ``yourbrand-carts-api``

Input these in the portal:

```
Name: get-cart-by-id-rule
Type: Custom
Metadata:
   - messageCount: 1
   - namespace: yourbrand-servicebus
   - queue: get-cart-by-id
```