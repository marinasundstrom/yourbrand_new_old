# Logging

In most services we use Serilog to produce structured logs, in JSON format.

Logs are being sent to Loki, and can be viewed in Grafana.

## Adjusted log levels

In order to not get spammed by metrics output in the console, the log levels for ``StoreWeb`` and ``AdminWeb`` have been set to ``Warning``.

These can be overridden during development in ``appsettings.Development.json``.