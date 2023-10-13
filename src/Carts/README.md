# Carts

## Seeding test data

```
dotnet run --project Carts.API -- --seed
```

This will create a sample cart with id "test". It is currently being used during development.

### Seeding remote database (such as in Azure SQL database)

In ``launchSettings.json`` , temporarily change the value of key ``ConnectionStrings__CartsDb`` to desired connection string - like the remote SQL Server. _(Default is local)_

Then execute:

```
dotnet run --project Carts.API -- --seed
```

Revert the changes to restore connection string to local database.

#### To do

Need to make sure that the product images have the correct URLs for remote.