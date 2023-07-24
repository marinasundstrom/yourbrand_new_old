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

### Develop Store Web against services running locally

For when you need to make changes to other services as well.

```
dotnet run --launch-profile "Development (Local Catalog API)"
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