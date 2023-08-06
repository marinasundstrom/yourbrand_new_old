# YourBrand

## Run

Run the entire system.

### Production mode

```
docker compose up
```

To run detached from the console:

```
docker compose up -d
```

### Development mode

```
docker compose -f docker-compose.debug.yml up
```

To rebuild all images:

```
docker compose -f docker-compose.debug.yml up --build
```

## Development

The entire project can be run locally either fully in Docker, or with some services running separately during development.

Everything has been pre-configured to work with a local SQL database as well as other dependencies running in Docker.

To start the database and other dependencies, run this:

```
docker compose -f docker-compose.deps.yml up -d
```

### Develop Store Web against services running locally

For when you need to make changes to other services as well.

```
dotnet run --launch-profile "Development (Local Catalog API)"
```

Or simply:

```
dotnet run
```

Requires the project ``Catalog.API`` to be started. It is dependant on the database to run.

### Develop Store Web locally against services running in Docker

For when you make changes just to Store Web.

```
dotnet run --launch-profile "Development (Catalog API in Docker)"
```

Requires Docker Compose file ``docker-compose.debug.yml`` to run.

### Develop Store Web locally against services running in the cloud

For when you need to do something to Store Web that requires data from the cloud.

```
dotnet run --launch-profile "Development (Remote Catalog API)"
```
## Deploy to Azure using GitHub Actions

Requires you to add secrets to your GitHub projects.

Every project in this repository should have its own workflow file that execute when they change.

In order for deployment to work, you need to make sure that the credentials (added to secrets) are correct. 

You also need to make sure that the container apps that you are deploying to exist in Azure.


## Azure environment

### Container Registry

A container registry.

### KeyVault

A KeyVault named: ``yourbrand-keyvault``

### Container apps

Create a container app environment with containers:

* ``yourbrand-store-web``
* ``yourbrand-catalog-api``
* ``yourbrand-carts-api``

### SQL Databases

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

### Create Service Bus

Name it ``yourbrand-servicebus``. 

Choose Pricing Tier "Standard" or above.

### Assigning managed identities

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

### Add secrets to KeyVault

Add these secrets with values

* ``yourbrand-catalog-api-url``
* ``yourbrand-carts-api-url``
* ``yourbrand-catalog-db-connectionstring``
* ``yourbrand-carts-db-connectionstring``
* ``yourbrand-servicebus-connectionstring``

Container registry:

And, the container apps need permissions to pull images from the container registry.

### Scale rule for ``yourbrand-carts-api``

Input these in the portal:

```
Name: get-cart-by-id-rule
Type: Custom
Metadata:
   - messageCount: 1
   - namespace: yourbrand-servicebus
   - queue: get-cart-by-id
```

## Run projects from CLI

```
dotnet run src/Store/Web/Server
dotnet run src/Admin/Web/Server
dotnet run src/Catalog/Catalog.API
dotnet run src/Carts/Carts.API
```

## Verify GitHub Actions with Act

To verify GitHub Actions, install [Act](https://github.com/nektos/act).

```
act -j <job-name>  -W .github/workflows/<workflow-file>.yaml
``````

## Structured data

We are using the schemas for [Product](https://schema.org/Product) and [Offer](https://schema.org/Offer).

Validate the usage with the 
[Validator](https://validator.schema.org/).
