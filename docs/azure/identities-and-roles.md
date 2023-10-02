# Identities and Roles in Azure

This project uses System-assigned managed identities and roles to authorize resources and users to access other resources within Azure.

You also need to add yourself as a member for certain resources in order to view, and update their data.

Some resources migh have more permissions than they need. This will be reviewed and changed in the future.

## How to enable managed identities and roles

Managed identities are enabled in for each service in their "Identity" blade, for services where it has to be explicitly enabled.

Enable "System-assigned managed identity".

You are now able to manage the roles for each service in their respective "Access control (IAM)" blades.

## Container registry access

The container apps need roles to pull images from the container registry.

Assign yourself as a ``Contributor`` so that you will be able to view and edit the contents in the Azure Portal.

Then assign the role ``AcrPull`` to allow container apps to pull images from ACR.

| Resource                   | Role    |
|--------                    |----     |
| ``yourbrand-store-web``    | AcrPull |
| ``yourbrand-admin-web``    | AcrPull |
| ``yourbrand-catalog-api``  | AcrPull |
| ``yourbrand-carts-api``    | AcrPull |

## Key Vault access

Assign yourself as a ``Contributor`` so that you will be able to view and edit contents in the Azure Portal.

Then assign the role ``Key Vault Secrets User`` to resources according to this:

| Resource                   | Role                    |
|--------                    |----                     |
| ``yourbrand-store-web``    | Key Vault Secrets User  |
| ``yourbrand-admin-web``    | Key Vault Secrets User  |
| ``yourbrand-catalog-api``  | Key Vault Secrets User  |
| ``yourbrand-carts-api``    | Key Vault Secrets User  |

## Service bus access

To allow container apps to use ``yourbrand-servicebus``.

Then assign the role ``Azure Service Bus Data Owner`` to resources according to this:

| Resource                   | Role                          |
|--------                    |----                           |
| ``yourbrand-store-web``    | Azure Service Bus Data Owner  |
| ``yourbrand-admin-web``    | Azure Service Bus Data Owner  |
| ``yourbrand-catalog-api``  | Azure Service Bus Data Owner  |
| ``yourbrand-carts-api``    | Azure Service Bus Data Owner  |

These permissions are required so that the container apps can set up topics and subscriptions.

## SQL Server and database access

Roles are to be assigned on the SQL Server.

Assign yourself as an admin /owner.

Make sure password is enabled to run migrations as part of GitHub Actions.

### Roles

Assign these role ``Contributor`` to the following Container apps:

| Resource                   | Role          |
|--------                    |----           |
| ``yourbrand-catalog-api``  | Contributor   |
| ``yourbrand-carts-api``    | Contributor   |

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

Then assign the role ``Storage Blob Data Owner`` to consuming resources:

| Resource                   | Role                      |
|--------                    |----                       |
| ``yourbrand-catalog-api``  | Storage Blob Data Owner   |

Enable anonymous access on storage account.

Then enable read access on container - so that blobs can be viewed via their public URLs.
