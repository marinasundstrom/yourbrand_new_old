# Production mode

To run in production mode on your local computer:


```
az login
az account list
az account get-access-token
```

```
ASPNETCORE_ENVIRONMENT=Production dotnet run -c Release --project src/Sales/Sales.API
```