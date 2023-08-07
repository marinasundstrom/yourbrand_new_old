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

To rebuild all images:

```
docker compose -f docker-compose.debug.yml up --build
```