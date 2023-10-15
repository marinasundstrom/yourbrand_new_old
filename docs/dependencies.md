# Dependencies

These are the dependencies that this project, and all of its services have.


|  Type                    | Azure (Resource)            |                            | Locally (Docker)              | 
|--------                  |--------                     |--                          |--------                       |  
| Message bus              | ``Azure Service Bus``       | ``yourbrand-servicebus``   | ``RabbitMQ``                  |
| Database server (SQL)    | ``Azure SQL Server``        | ``yourbrand-sqlserver``    | ``Azure SQL Edge``            |
| Storage (Blob)           | ``Azure Storage Account``   | ``yourbrandstorage``       | ``Azurite Storage Emulator``  |


## Dependants

For observability:

* Grafana
* Prometheus
* Loki

At the moment, only running locally.

## Settings and Secrets

Azure KeyVault is used to store secrets in the cloud.

Locally, settings are injected into service as environment variables. When running a service as is, then they are defined in the ``Properties/launchSettings.json`` of each project.

If you run teh services in Docker, then check the Docker compose file for the environment variables.
