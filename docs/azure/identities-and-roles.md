# Identities and Roles in Azure

This project uses System-assigned managed identities and roles to authorize resources and users to access other resources within Azure.

You also need to add yourself as a member for certain resources in order to view, and update their data.

## How to enable managed identities and roles

Managed identities are enabled in for each service in their "Identity" blade, for services where it has to be explicitly enabled.

Enable "System-assigned managed identity".

You are now able to manage the roles for each service in their respective "Access control (IAM)" blades.

## Container registry access

The container apps need roles to pull images from the container registry.

Assign yourself as a ``Contributor`` so that you will be able to view and edit the contents in the Azure Portal.

Assign role ``AcrPull`` to all container apps so that they can pull the images.

## Key Vault access

Assign yourself as a ``Contributor`` so that you will be able to view and edit contents in the Azure Portal.

Assign the role ``Key Vault Secrets User`` in ``yourbrand-keyvault`` to all Container apps.

## Service bus access

Assign the following roles in ``yourbrand-servicebus`` to all Container apps:

* ``Azure Service Bus Data Owner`` (?)
* ``Azure Service Bus Data Receiver``
* ``Azure Service Bus Data Sender``

## SQL Server and databases

Roles are to be assigned on the SQL Server.

Assign yourself as an admin /owner.

Make sure password is enabled to run migrations as part of GitHub Actions.

### Roles

Assign these role ``Contributor`` to the following Container apps:

* ``yourbrand-catalog-api``
* ``yourbrand-carts-api``

_This list might change as more  container apps and databases are added._

### Create database users for services

Provided that the services have got permission to the SQL Server.

Create users for each container app to each DB:

In ``yourbrand-catalog-db`` run this to create user for ``yourbrand-catalog-api``:

```sql
CREATE USER [yourbrand-catalog-api] FROM EXTERNAL PROVIDER
ALTER ROLE db_datareader ADD MEMBER [yourbrand-catalog-api]
ALTER ROLE db_datawriter ADD MEMBER [yourbrand-catalog-api]
```

And, in ``yourbrand-carts-db`` run this to create user for ``yourbrand-carts-api``:

```sql
CREATE USER [yourbrand-carts-api] FROM EXTERNAL PROVIDER
ALTER ROLE db_datareader ADD MEMBER [yourbrand-carts-api]
ALTER ROLE db_datawriter ADD MEMBER [yourbrand-carts-api]
```

## Storage Account access

Assign yourself as a ``Contributor`` to be able to view and edit contents in the Azure Portal.

``yourbrand-catalog-api`` should be a ``Storage Blob Data Owner``.

Enable anonymous access on storage account.

Then enable read access on container - so that blobs can be viewed via their public URLs.
