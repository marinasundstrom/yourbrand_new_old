# Identities and Permissions in Azure

Make sure to enable Managed Identity  for each resource. 

This project uses System-assigned managed identities.

Setting up managed identities, and the permissions for resources accessing other resources.

## Container registry access

The container apps need permissions to pull images from the container registry.

## Key Vault access

*  Both ``yourbrand-catalog-api``, ``yourbrand-carts-api``,and ``yourbrand-store-web`` should be 
"Key Vault Secrets User" to ``yourbrand-keyvault``

## Database access

* ``yourbrand-catalog-api`` should have read write access to ``yourbrand-catalog-db``
* ``yourbrand-carts-api`` should have read write access to ``yourbrand-carts-db``

## Service bus access

Both ``yourbrand-catalog-api``, ``yourbrand-carts-api``, and ``yourbrand-store-web`` should have these roles to ``yourbrand-servicebus``:

* Azure Service Bus Data Owner
* Azure Service Bus Data Receiver
* Azure Service Bus Data Sender

## SQL database access

Each service should have read write access to the databases they use - and only those they use.

### Create database users

Create users for each container app to each DB:

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

## Storage Account access

``yourbrand-catalog-api`` should have access to ``yourbrandstorage``