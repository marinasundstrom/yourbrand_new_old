# Set up Azure environment

This can be done from the portal:

## Install Azure CLI

Make sure that Azure CLI has been installed.

Then log in:

```
az login
```

## Create resources

Create resources listed [here](resources.md).

## Assigning managed identities

Described [here](identities-and-roles.md).

## Add secrets to KeyVault

Add the [secrets](secrets.md) to KeyVault.

## Scale rule for ``yourbrand-carts-api``

Input these in the portal:

```
Name: get-cart-by-id-rule
Type: Custom
Custom Rule Type: azure-servicebus
Metadata:
   - messageCount: 1
   - namespace: yourbrand-servicebus
   - queue: get-cart-by-id
```
