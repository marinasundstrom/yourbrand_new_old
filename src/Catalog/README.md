# Catalog

## Seeding test data

```
dotnet run --project Catalog.API -- --seed
```

```
dotnet ef migrations add <Name>
```

```
dotnet ef database update --connection "Server=localhost,1433;User Id=sa;Password=P@ssw0rd;Encrypt=false;Database=yourbrand-catalog-db"
```

_* This is the connection string to "yourbrand-catalog-db" running locally in Docker._