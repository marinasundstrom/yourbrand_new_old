# Service Authentication and Authorization

This controls what users do, and what services can do to with each other. For example, what APIs the user can invoke, and what services can invoke APIs of other services.

## Environments

When in development we use Duende IdentityServer for authentication.

When in production we use Entra ID (formerly Azure (AD) Active Directory).

These are further explained in separated documents.

## Applications

### StoreFront

StoreFront is a public API that the Store site may access without authenticating.

At startup, the StoreFront service obtains an Access Token in return for a Client Secret that is used to access the Catalog Service, among other services.

### Catalog

StoreFront is an authorized application to Catalog API.

### Admin Service (Client & Server)

The user logs in and may access Catalog and other services.