# Service auth with Identity Service

``appsettings.Development.json``

```json
{
  "Local": {
    "Authority": "https://localhost:5041",
    "Audience": "salesapi"
  }
}
```

``https://localhost:5041`` is the address of Identity Service.

## API Resource

You have to define the API Resource ``salesapi`` (or whatever you call it) for your resource.

The API resource has to be registered  added to the client credentials in IdentityService.