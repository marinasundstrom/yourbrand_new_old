# Identities and Roles in Azure

Make sure to enable Managed Identity for each resource. 

This project uses System-assigned managed identities.

Setting up managed identities, and the roles for resources accessing other resources.

You need to add yourself as a member for certain resources in order to view, and update their data.

## Container registry access

The container apps need roles to pull images from the container registry.

Assign yourself as a ``Contributor``.

Assign role ``AcrPull`` to all container apps.

## Key Vault access

Assign yourself as a ``Contributor``.

*  Both ``yourbrand-catalog-api``, ``yourbrand-carts-api``,and ``yourbrand-store-web`` should be ``Key Vault Secrets User`` to ``yourbrand-keyvault``

## Service bus access

Both ``yourbrand-catalog-api``, ``yourbrand-carts-api``, and ``yourbrand-store-web`` should have these roles to ``yourbrand-servicebus``:

* Azure Service Bus Data Owner
* Azure Service Bus Data Receiver
* Azure Service Bus Data Sender

## SQL database

Roles are to be assigned on the SQL Server.

Assign yourself as an admin /owner.

Make sure password is enabled to run migrations as part of GitHub Actions.

### Roles

Assign these roles to your services:

* ``yourbrand-catalog-api`` should be a ``Contributor``.
* ``yourbrand-carts-api`` should be a ``Contributor``.

### Create database users for services

Provided that the services have got permission to the SQL Server.

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

Set yourself as a ``Contributor``.

``yourbrand-catalog-api`` should be a ``Storage Blob Data Owner``.

Enable anonymous access on storage account.

Then enable read access on container - so that blobs can be viewed via their public URLs.
