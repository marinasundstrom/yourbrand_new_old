#!sh

RESOURCE_GROUP="YourBrand2"
LOCATION="swedencentral"

az group create --name $RESOURCE_GROUP --location $LOCATION

az deployment group create --resource-group $RESOURCE_GROUP --template-file "./bicep/main.bicep" --parameters "./bicep/main.parameters.json"