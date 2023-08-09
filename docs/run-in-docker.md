# Run in Docker

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

To rebuild the images of all services:

```
docker compose -f docker-compose.debug.yml up --build
```

To rebuild the image of a specific service:

```
docker compose -f docker-compose.debug.yml up --build <service name>
```

### Run just the dependencies

This starts the local dependencies mentioned [here](dependencies.md).

```
docker compose -f docker-compose.deps.yml up
```