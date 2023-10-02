# Bicep

Bicep is a tool that allows you can define Infrastructure as code (IaC) in Azure. 

Meaning that we define the environment and the resources in it using a domain-specific language, Bicep.

With Bicep you can quickly deploy new resources, or make change existing resources.

Bicep is a more human-friendly alternative to the JSON-based ARM templates.

## Use Bicep

### Set environment variables

```sh
$RESOURCE_GROUP="yourbrand"
$LOCATION="swedencentral"
```

### Create Resource Group

```sh
az group create `
--name $RESOURCE_GROUP `
--location $LOCATION
```

### Create deployment

```sh
az deployment group create `
--resource-group $RESOURCE_GROUP `
--template-file "./bicep/main.bicep" `
--parameters "./bicep/main.parameters.json"
```

## Export and Decompile ARM templates to Bicep

_This is useful when already having resources in Azure._

```sh
az group export --name "YourBrand" > main.json
az bicep decompile --file main.json
```