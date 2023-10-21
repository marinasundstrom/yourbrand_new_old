# Services

These are the services in this project:

| Common name    | Project                  | Service name / Azure resource   | Type    |
|--------        |--------                  |----                             |---      |         
| ``StoreWeb``   | ``Store/Web``            | ``yourbrand-store-web``         | Web app |
| ``AdminWeb``   | ``Admin/Web``            | ``yourbrand-admin-web``         | Web app |
| ``Catalog``    | ``Catalog/Catalog.API``  | ``yourbrand-catalog-svc``       | Service |
| ``Carts``      | ``Carts/Carts.API``      | ``yourbrand-carts-svc``         | Service |

## Dependencies

These are the dependencies that this project, and all of its services have.

|  Type                    | Azure resource              |                            | Local (Docker)              | 
|--------                  |--------                     |--                          |--------                       |  
| Message bus              | ``Azure Service Bus``       | ``yourbrand-servicebus``   | ``RabbitMQ``                  |
| Database server (SQL)    | ``Azure SQL Server``        | ``yourbrand-sqlserver``    | ``Azure SQL Edge``            |
| Storage (Blob)           | ``Azure Storage Account``   | ``yourbrandstorage``       | ``Azurite Storage Emulator``  |

## Service discovery

We use service discovery to automatically register services by their name.

Then we use this convention to connect to a specific service: ``http://<service-name>``.

The service names are the same as the resource names in Azure.

Azure Container apps support service discovery. 

Locally, in development, we are using Consul.

## Dependants

For observability:

* Grafana
* Prometheus
* Loki

At the moment, only running locally.

## Settings and Secrets

Azure KeyVault is used to store secrets in the cloud.

App settings are stored in Azure App Configuration.

Locally, settings are injected into service as environment variables. When running a service as is, then they are defined in the ``Properties/launchSettings.json`` of each project.

If you run teh services in Docker, then check the Docker compose file for the environment variables.
