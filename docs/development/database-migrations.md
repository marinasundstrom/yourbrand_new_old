# Database Migrations 

Migration will automatically run with deployment.

But if you need to run them locally, these commands might be useful:

```
dotnet ef database update -p src/Catalog/Catalog.API --connection <connection-string>
dotnet ef database update -p src/Carts/Carts.API --connection <connection-string>
```