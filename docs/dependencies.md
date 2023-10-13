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