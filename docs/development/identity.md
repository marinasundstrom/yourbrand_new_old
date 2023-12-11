# Identity

We use Duende Identity Server when developing.

## Identities

The identities and passwords are:

```
Username: Alice
Password: Pass123$

Username: Bob
Password: Pass123$
``````

## Migrations

```
rm -rf "Data/Migrations"

dotnet ef migrations add Users -c ApplicationDbContext -o Data/Migrations
```

## Seed

```
dotnet run -- /seed
```