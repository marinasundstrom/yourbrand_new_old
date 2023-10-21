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

To start the database and other [dependencies](services.md), run this:

```
docker compose -f docker-compose.deps.yml up -d
```

## Set up Azurite

To upload and serve blobs (product images) you need to configure Azurite.

More on that [here](set-up-azurite.md).

## Run individual services from CLI

You can start individual services using these commands:

```sh
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

## Seeding data (Dev)

### Databases

```sh
dotnet run --project src/Carts/Carts.API --seed
dotnet run --project src/Catalog/Catalog.API --seed
```

### Blob Storage

To seed product images, follow [this guide](/docs/seed/blobs/README.md).