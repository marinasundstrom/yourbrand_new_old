# Service auth with Entra (Azure AD)

For more general info about service auth, check [this](/docs/service-auth.md).

## Concepts

### App registrations

These correspond to applications, or services.

### Permissions

Permissions are assigned to scopes, and then granted consent.

There are two types of permissions:

* Delegated - Using the credentials of a logged in user
* Application - Credentials belonging to an application - Client Secret or Certificate

### Scopes

Allows you to subdivide an API, and in effect so that only certain clients may access parts of the functionality.

## App registrations

These correspond to applications, or services, in this solution.

### Admin Client

### Admin Service

### Catalog

### StoreFront

This is a public frontend API.

The StoreFront service obtains Access Token in return for a Client Secret at startup that is used to access the Catalog Service, among other services.

### Configuration

``appsettings.Production.json``

```json
{
    "AzureAd": {
        "Instance": "https://login.microsoftonline.com/",
        "ClientId": "6aebaf1f-7160-4a72-bb48-034e661e8c7b",
        "TenantId": "common",
        "Audience": "api://6aebaf1f-7160-4a72-bb48-034e661e8c7b"
    },
}
``````