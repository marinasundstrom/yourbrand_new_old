# Identity Service

We use Duende Identity Server when developing, as a stand-in for Entra.

For more general info about service auth, check [this](/docs/service-auth.md).

## Identities

The identities and passwords are:

```
Username: Alice
Password: Pass123$

Username: Bob
Password: Pass123$
``````

This can be changed in SeedData.cs.

## Concepts

### Access Token

A token that can be used to access a resource. Contains a signature that can be verified by both issuer and consumer independently.

Contains Claims.

Normally in the form of a JSON Web Token (JWT) which is a Base64 encoded JSON string.

There is also Cookie Auth.

### Identity Token

Mostly synonymous with Access Token.

### Claims

Individual pieces of information to be included in the access token, as a name/key with a value.

These might be standardized info such as Given Name or Role. But you can add your own.

### Identity Resources

A specified set of information belonging to an identity that will be included with the credentials. This information is referred to as "claims".

### API Resources

This is simply and API.

It has at least one or more API Scopes.

It also may have specified claims that will be included in an access token.

### API Scopes

Scopes allow you to subdivide the functionality of an API. So that a certain client only may access certain parts of it.

### Clients

These are the applications that are consuming API resources.

A client may have different ways of authenticating - referred to as "Grant types".

The most common way is for users to be logging in with username and password, to then get a token to access with which to access API resources.

If the client is an application that is not tied to a user, then you can opt for providing Client Credentials, with a Client Secret, in return for the token.

You can limit a client to a specific set of allowed scopes.

## Files of interest

The resources, scopes, and clients are defined in [Config.cs](src/IdentityService/Config.cs) and loaded into memory at each startup.


The Users are defined in [SeedData.cs](src/IdentityService/SeedData.cs) and seeded to the database.


## Migrations

```
rm -rf "Data/Migrations"

dotnet ef migrations add Users -c ApplicationDbContext -o Data/Migrations
```

## Seed

To seed data.

```
dotnet run -- /seed
```