# Running in Docker

## Production mode

This will run the app in production mode, which requires access and permissions to Azure resources.

```
docker compose up
```

To run detached from the console:

```
docker compose up -d
```

## Development mode

You can run everything, including dev dependencies, just by running this command:

```
docker compose -f docker-compose.debug.yml up
```

To (re)build the images of all services:

```
docker compose -f docker-compose.debug.yml build
```

To rebuild the image of a specific service that is running:

```
docker compose -f docker-compose.debug.yml up --build <service name>
```

## Run just the dependencies

This starts the local dependencies mentioned [here](services.md).

```
docker compose -f docker-compose.deps.yml up
```

## Service DNS

Inside Docker you may use ``host.docker.internal:<port>`` or ``<service-name>:<port>`` to address a service.

While from the outside you use ``localhost:<port>``.

## Issue with service discovery

<strike>For ``docker-compose.debug.yml`` we have disabled service discovery, since it doesn't work with containerized services. Consul will register the internal port (8080), and not the external.</strike>

Services are registered with assigned hostname and port.

We pass the service URLs as configuration through environment variables.