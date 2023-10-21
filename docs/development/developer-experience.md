# Developer experience

The entire project can be run locally either fully in Docker, or with some services running separately during development.

Everything has been pre-configured to work with a local SQL database as well as other dependencies running in Docker.

## Prerequisites

The following software has to be present on your development machine in order to build and run this project:

* .NET SDK 8
* Docker desktop

## VS Code extensions

Run the ``install-vscode-extensions.sh`` script to install some useful extensions, like _Restore Terminals_.

**Restore Terminals** will make it easier for you to start up all services locally by automatically creating a terminal window for each of them.

## Run dependencies in Docker

To start the database and other [dependencies](dependencies.md), run this:

```
docker compose -f docker-compose.deps.yml up -d
```

## Set up Azurite

To upload and serve blobs (product images) you need to configure Azurite.

More on that [here](set-up-azurite.md).

## Run individual services from CLI

You can start individual services using these commands:

```
dotnet run --project src/Store/Web/Server
dotnet run --project src/Admin/Web/Server
dotnet run --project src/Catalog/Catalog.API
dotnet run --project src/Carts/Carts.API
```

### VS Code

You can use the [Restore Terminal](https://marketplace.visualstudio.com/items?itemName=EthanSK.restore-terminals) VS Code extension to restore Terminal windows with these commands.

The terminals are configured in the ``.vscode/settings.json`` file.

## Service hosts

* Store Web: https://localhost:7188/
* Admin Web: https://localhost:5001/
* Catalog https://localhost:7134/
* Carts: https://localhost:7154/

Each services has a Swagger UI endpoint at ``/swagger``.

## Developing the Store Web

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