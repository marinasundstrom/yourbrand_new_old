# Development environment

## Tools

* .NET 8 SDK
* VS Code ([About the environment](/docs/development/vs-code.md))
* Docker Desktop

## Docker

Main dependencies that run in Docker.

* Azure SQL Edge (Server) - SQL database
* Azurite Storage Emulator - Blob storage
* RabbitMQ - Message broker
* Consul - Allows for service discovery

Observability:

* Zipkin - UI for distributed tracing
* Grafana - UI for displaying metrics, and logs
* Prometheus - Collects metrics
* Loki - Collects logs

Using OpenTelemetry.