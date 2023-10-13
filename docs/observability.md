# Observability

To monitor instance, see metrics, and view logs in Grafana.

Using Prometheus and Loki.

## Monitored services

* ``StoreWeb``
* ``AdminWeb``

## Local development with Docker

Grafana is one of the dependencies running in Docker Compose.

URL: [http://localhost:3000](http://localhost:3000)

```
Username: admin
Password: grafana
```

_Credentials are specified in the "docker-compose.deps.yml" file._