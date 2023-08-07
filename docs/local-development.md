# Local development

The entire project can be run locally either fully in Docker, or with some services running separately during development.

Everything has been pre-configured to work with a local SQL database as well as other dependencies running in Docker.

To start the database and other dependencies, run this:

```
docker compose -f docker-compose.deps.yml up -d
```

## Run individual services from CLI

You can start individual services using these commands:

```
dotnet run --project src/Store/Web/Server
dotnet run --project src/Admin/Web/Server
dotnet run --project src/Catalog/Catalog.API
dotnet run --project src/Carts/Carts.API
```

## Common development scenarios

### Developing Store Web against services running locally

For when you need to make changes to other services as well.

```
dotnet run --launch-profile "Development (Local Catalog API)"
```

Or simply:

```
dotnet run
```

Requires the project ``Catalog.API`` to be started. It is dependant on the database to run.

### Developing Store Web locally against services running in Docker

For when you make changes just to Store Web.

```
dotnet run --launch-profile "Development (Catalog API in Docker)"
```

Requires Docker Compose file ``docker-compose.debug.yml`` to run.

### Developing Store Web locally against services running in the cloud

For when you need to do something to Store Web that requires data from the cloud.

```
dotnet run --launch-profile "Development (Remote Catalog API)"
```