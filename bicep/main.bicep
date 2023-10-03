@description('The location where the resources will be created.')
param location string = resourceGroup().location

@description('Optional. The prefix to be used for all resources created by this template.')
param prefix string = ''

@description('Optional. The suffix to be used for all resources created by this template.')
param suffix string = ''

@description('Optional. The tags to be assigned to the created resources.')
param tags object = {}

param adminWebName string
param cartsApiName string
param catalogApiName string
param storeWebName string

param containerAppEnvironmentName string
param serviceBusName string
param containerRegistryName string
param sqlServerName string
param storageAccountName string
param keyVaultName string

param vNetName string
var subnetName = 'backendSubnet'
var subnetRef = resourceId('Microsoft.Network/virtualNetworks/subnets', vNetName, subnetName)

param logAnalyticsWorkspaceName string =  '${prefix}log-${uniqueString(resourceGroup().id)}${suffix}'

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: containerRegistryName
  properties: {
    adminUserEnabled: true
    dataEndpointEnabled: false
    encryption: {
      status: 'disabled'
    }
    networkRuleBypassOptions: 'AzureServices'
    policies: {
      exportPolicy: {
        status: 'enabled'
      }
      quarantinePolicy: {
        status: 'disabled'
      }
      retentionPolicy: {
        days: 7
        status: 'disabled'
      }
      trustPolicy: {
        status: 'disabled'
        type: 'Notary'
      }
    }
    publicNetworkAccess: 'Enabled'
    zoneRedundancy: 'Disabled'
  }
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-02-01' = {
  location: location
  name: keyVaultName
  properties: {
    accessPolicies: []
    enableRbacAuthorization: true
    enableSoftDelete: true
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: false
    provisioningState: 'Succeeded'
    publicNetworkAccess: 'Enabled'
    sku: {
      family: 'A'
      name: 'standard'
    }
    softDeleteRetentionInDays: 90
    tenantId: '99305724-84f9-474d-aa7b-759e7b4d38d2'
    vaultUri: 'https://${keyVaultName}.vault.azure.net/'
  }
}

resource vNet 'Microsoft.Network/virtualNetworks@2023-05-01' = {
  location: location
  name: vNetName
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.0.0.0/16'
      ]
    }
    enableDdosProtection: false
    subnets: [
      {
        id: subnetRef
        name: 'infra-subnet'
        properties: {
          addressPrefix: '10.0.0.0/23'
          delegations: [
            {
              id: '${subnetRef}/delegations/Microsoft.App.environments'
              name: 'Microsoft.App.environments'
              properties: {
                serviceName: 'Microsoft.App/environments'
              }
              type: 'Microsoft.Network/virtualNetworks/subnets/delegations'
            }
          ]
          privateEndpointNetworkPolicies: 'Disabled'
          privateLinkServiceNetworkPolicies: 'Enabled'
          serviceEndpoints: []
        }
        type: 'Microsoft.Network/virtualNetworks/subnets'
      }
    ]
    virtualNetworkPeerings: []
  }
}

resource serviceBus 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' = {
  location: location
  name: serviceBusName
  properties: {
    disableLocalAuth: false
    minimumTlsVersion: '1.2'
    premiumMessagingPartitions: 0
    publicNetworkAccess: 'Enabled'
    zoneRedundant: false
  }
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
}

resource sqlServer 'Microsoft.Sql/servers@2023-02-01-preview' = {
  identity: {
    type: 'SystemAssigned'
  }
  kind: 'v12.0'
  location: location
  name: sqlServerName
  properties: {
    administratorLogin: 'CloudSA88e7889a'
    administrators: {
      administratorType: 'ActiveDirectory'
      azureADOnlyAuthentication: false
      login: 'robert.sundstrom_outlook.com#EXT#@robertsundstromoutlook.onmicrosoft.com'
      principalType: 'User'
      sid: 'a37f7582-181d-46d5-8403-f45c75dd39b2'
      tenantId: '99305724-84f9-474d-aa7b-759e7b4d38d2'
    }
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    restrictOutboundNetworkAccess: 'Disabled'
    version: '12.0'
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  kind: 'StorageV2'
  location: location
  name: storageAccountName
  properties: {
    accessTier: 'Hot'
    allowBlobPublicAccess: true
    allowCrossTenantReplication: false
    allowSharedKeyAccess: true
    defaultToOAuthAuthentication: false
    dnsEndpointType: 'Standard'
    encryption: {
      keySource: 'Microsoft.Storage'
      requireInfrastructureEncryption: false
      services: {
        blob: {
          enabled: true
          keyType: 'Account'
        }
        file: {
          enabled: true
          keyType: 'Account'
        }
      }
    }
    minimumTlsVersion: 'TLS1_2'
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Allow'
      ipRules: []
      virtualNetworkRules: []
    }
    publicNetworkAccess: 'Enabled'
    supportsHttpsTrafficOnly: true
  }
  sku: {
    name: 'Standard_RAGRS'
    tier: 'Standard'
  }
}

resource adminWeb 'Microsoft.App/containerapps@2023-05-02-preview' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: adminWebName
  properties: {
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        allowInsecure: false
        exposedPort: 0
        external: true
        targetPort: 8080
        traffic: [
          {
            latestRevision: true
            weight: 100
          }
        ]
        transport: 'Auto'
      }
      registries: [
        {
          server: '${containerRegistryName}.azurecr.io'
          identity: 'system'
        }
      ]
      secrets: [
      ]
    }
    environmentId: containerAppEnvironment.id
    managedEnvironmentId: containerAppEnvironment.id
    template: {
      containers: [
        {
          image: 'yourbrandcr.azurecr.io/github-action/container-app:6372877077.1'
          name: adminWebName
          probes: []
          resources: {
            cpu: '0.5'
            memory: '1Gi'
          }
        }
      ]
      scale: {
        maxReplicas: 10
        minReplicas: 0
      }
      volumes: []
    }
    workloadProfileName: 'Consumption'
  }
}

resource cartsApi 'Microsoft.App/containerapps@2023-05-02-preview' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: cartsApiName
  properties: {
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        allowInsecure: false
        exposedPort: 0
        external: true
        targetPort: 8080
        traffic: [
          {
            latestRevision: true
            weight: 100
          }
        ]
        transport: 'Auto'
      }
      registries: [
        {
          server: '${containerRegistryName}.azurecr.io'
          identity: 'system'
        }
      ]
      secrets: [
      ]
    }
    environmentId: containerAppEnvironment.id
    managedEnvironmentId: containerAppEnvironment.id
    template: {
      containers: [
        {
          image: 'yourbrandcr.azurecr.io/github-action/container-app:6373169170.1'
          name: cartsApiName
          probes: []
          resources: {
            cpu: '0.5'
            memory: '1Gi'
          }
        }
      ]
      scale: {
        maxReplicas: 10
        minReplicas: 0
        rules: [
          {
            custom: {
              metadata: {
                messageCount: '1'
                namespace: 'yourbrand-servicebus'
                queue: 'get-cart-by-id'
              }
              type: 'azure-servicebus'
            }
            name: 'get-cart-by-id-rule'
          }
        ]
      }
      volumes: []
    }
    workloadProfileName: 'Consumption'
  }
}

resource catalogApi 'Microsoft.App/containerapps@2023-05-02-preview' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: catalogApiName
  properties: {
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        allowInsecure: false
        exposedPort: 0
        external: true
        targetPort: 8080
        traffic: [
          {
            latestRevision: true
            weight: 100
          }
        ]
        transport: 'Auto'
      }
      registries: [
        {
          server: '${containerRegistryName}.azurecr.io'
          identity: 'system'
        }
      ]
      secrets: [
      ]
    }
    environmentId: containerAppEnvironment.id
    managedEnvironmentId: containerAppEnvironment.id
    template: {
      containers: [
        {
          image: 'yourbrandcr.azurecr.io/github-action/container-app:6372877073.1'
          name: catalogApiName
          probes: []
          resources: {
            cpu: '0.5'
            memory: '1Gi'
          }
        }
      ]
      scale: {
        maxReplicas: 10
        minReplicas: 0
      }
      volumes: []
    }
    workloadProfileName: 'Consumption'
  }
}

resource storeWeb 'Microsoft.App/containerapps@2023-05-02-preview' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: storeWebName
  properties: {
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        allowInsecure: false
        clientCertificateMode: 'Ignore'
        exposedPort: 0
        external: true
        stickySessions: {
          affinity: 'sticky'
        }
        targetPort: 8080
        traffic: [
          {
            latestRevision: true
            weight: 100
          }
        ]
        transport: 'Auto'
      }
      registries: [
        {
          server: '${containerRegistryName}.azurecr.io'
          identity: 'system'
        }
      ]
      secrets: [
      ]
    }
    environmentId: containerAppEnvironment.id
    managedEnvironmentId: containerAppEnvironment.id
    template: {
      containers: [
        {
          image: 'yourbrandcr.azurecr.io/github-action/container-app:6376722172.1'
          name: storeWebName
          probes: []
          resources: {
            cpu: '0.5'
            memory: '1Gi'
          }
        }
      ]
      scale: {
        maxReplicas: 10
        minReplicas: 0
      }
      volumes: []
    }
    workloadProfileName: 'Consumption'
  }
}

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2023-05-02-preview' = {
  location: location
  name: containerAppEnvironmentName
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspace.properties.customerId
        sharedKey:  logAnalyticsWorkspace.listKeys().primarySharedKey
      }
    }
    customDomainConfiguration: {}
    daprConfiguration: {}
    infrastructureResourceGroup: 'ME_${containerAppEnvironmentName}_YourBrand_swedencentral'
    kedaConfiguration: {}
    peerAuthentication: {
      mtls: {
        enabled: false
      }
    }
    vnetConfiguration: {
      infrastructureSubnetId: subnetRef
      internal: false
    }
    workloadProfiles: [
      {
        name: 'Consumption'
        workloadProfileType: 'Consumption'
      }
    ]
    zoneRedundant: false
  }
}

resource registry_repositories_admin 'Microsoft.ContainerRegistry/registries/scopeMaps@2023-07-01' = {
  parent: containerRegistry
  name: '_repositories_admin'
  properties: {
    actions: [
      'repositories/*/metadata/read'
      'repositories/*/metadata/write'
      'repositories/*/content/read'
      'repositories/*/content/write'
      'repositories/*/content/delete'
    ]
    description: 'Can perform all read, write and delete operations on the registry'
  }
}

resource registry_repositories_pull 'Microsoft.ContainerRegistry/registries/scopeMaps@2023-07-01' = {
  parent: containerRegistry
  name: '_repositories_pull'
  properties: {
    actions: [
      'repositories/*/content/read'
    ]
    description: 'Can pull any repository of the registry'
  }
}

resource registry_repositories_push 'Microsoft.ContainerRegistry/registries/scopeMaps@2023-07-01' = {
  parent: containerRegistry
  name: '_repositories_push'
  properties: {
    actions: [
      'repositories/*/content/read'
      'repositories/*/content/write'
    ]
    description: 'Can push to any repository of the registry'
  }
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  tags: tags
  properties: any({
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  })
}

resource secret_carts_api_url 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  location: location
  name: 'yourbrand-carts-svc-url'
  properties: {
    value: cartsApi.properties.configuration.ingress.fqdn
    attributes: {
      enabled: true
    }
  }
}

resource secret_yourbrand_carts_db_connectionstring 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  location: location
  name: 'yourbrand-carts-db-connectionstring'
  properties: {
    value: 'Server=tcp:${sqlServer_yourbrand_carts_db.name}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=yourbrand-carts-db;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication="Active Directory Default";'
    attributes: {
      enabled: true
    }
  }
}

resource secret_yourbrand_catalog_api_url 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  location: location
  name: 'yourbrand-catalog-svc-url'
  properties: {
    value: catalogApi.properties.configuration.ingress.fqdn
    attributes: {
      enabled: true
    }
  }
}

resource secret_yourbrand_catalog_db_connectionstring 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  location: location
  name: 'yourbrand-catalog-db-connectionstring'
  properties: {
    value: 'Server=tcp:${sqlServer_yourbrand_carts_db.name}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=yourbrand-catalog-db;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication="Active Directory Default";'
    attributes: {
      enabled: true
    }
  }
}

resource secret_yourbrand_servicebus_connectionstring 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  location: location
  name: 'yourbrand-servicebus-connectionstring'
  properties: {
    value: 'sb://${serviceBus.name}.servicebus.windows.net'
    attributes: {
      enabled: true
    }
  }
}

resource vNet_infra_subnet 'Microsoft.Network/virtualNetworks/subnets@2023-05-01' = {
  parent: vNet
  name: 'infra-subnet'
  properties: {
    addressPrefix: '10.0.0.0/23'
    delegations: [
      {
        id: '${subnetRef}/delegations/Microsoft.App.environments'
        name: 'Microsoft.App.environments'
        properties: {
          serviceName: 'Microsoft.App/environments'
        }
        type: 'Microsoft.Network/virtualNetworks/subnets/delegations'
      }
    ]
    privateEndpointNetworkPolicies: 'Disabled'
    privateLinkServiceNetworkPolicies: 'Enabled'
    serviceEndpoints: []
  }
}

/*
resource sqlServer_ActiveDirectory 'Microsoft.Sql/servers/administrators@2023-02-01-preview' = {
  parent: sqlServer
  name: 'ActiveDirectory'
  properties: {
    administratorType: 'ActiveDirectory'
    login: 'robert.sundstrom_outlook.com#EXT#@robertsundstromoutlook.onmicrosoft.com'
    sid: 'a37f7582-181d-46d5-8403-f45c75dd39b2'
    tenantId: '99305724-84f9-474d-aa7b-759e7b4d38d2'
  }
}
*/

resource sqlServer_yourbrand_carts_db 'Microsoft.Sql/servers/databases@2023-02-01-preview' = {
  parent: sqlServer
  kind: 'v12.0,user'
  location: location
  name: 'yourbrand-carts-db'
  properties: {
    availabilityZone: 'NoPreference'
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    isLedgerOn: false
    maintenanceConfigurationId: '/subscriptions/0413e290-39df-4e4c-b58c-7814b4a5d7ac/providers/Microsoft.Maintenance/publicMaintenanceConfigurations/SQL_Default'
    maxSizeBytes: 1073741824
    readScale: 'Disabled'
    requestedBackupStorageRedundancy: 'Local'
    zoneRedundant: false
  }
  sku: {
    capacity: 5
    name: 'Basic'
    tier: 'Basic'
  }
}

resource sqlServer_yourbrand_catalog_db 'Microsoft.Sql/servers/databases@2023-02-01-preview' = {
  parent: sqlServer
  kind: 'v12.0,user'
  location: location
  name: 'yourbrand-catalog-db'
  properties: {
    availabilityZone: 'NoPreference'
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    isLedgerOn: false
    maintenanceConfigurationId: '/subscriptions/0413e290-39df-4e4c-b58c-7814b4a5d7ac/providers/Microsoft.Maintenance/publicMaintenanceConfigurations/SQL_Default'
    maxSizeBytes: 1073741824
    readScale: 'Disabled'
    requestedBackupStorageRedundancy: 'Local'
    zoneRedundant: false
  }
  sku: {
    capacity: 5
    name: 'Basic'
    tier: 'Basic'
  }
}

resource storageAccounts_yourbrandstorage_name_default 'Microsoft.Storage/storageAccounts/blobServices@2023-01-01' = {
  parent: storageAccount
  name: 'default'
  properties: {
    changeFeed: {
      enabled: false
    }
    containerDeleteRetentionPolicy: {
      days: 7
      enabled: true
    }
    cors: {
      corsRules: []
    }
    deleteRetentionPolicy: {
      allowPermanentDelete: false
      days: 7
      enabled: true
    }
    isVersioningEnabled: false
    restorePolicy: {
      enabled: false
    }
  }
  sku: {
    name: 'Standard_RAGRS'
    tier: 'Standard'
  }
}

resource Microsoft_Storage_storageAccounts_fileServices_storageAccounts_yourbrandstorage_name_default 'Microsoft.Storage/storageAccounts/fileServices@2023-01-01' = {
  parent: storageAccount
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
    protocolSettings: {
      smb: {}
    }
    shareDeleteRetentionPolicy: {
      days: 7
      enabled: true
    }
  }
  sku: {
    name: 'Standard_RAGRS'
    tier: 'Standard'
  }
}

resource Microsoft_Storage_storageAccounts_queueServices_storageAccounts_yourbrandstorage_name_default 'Microsoft.Storage/storageAccounts/queueServices@2023-01-01' = {
  parent: storageAccount
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}

resource Microsoft_Storage_storageAccounts_tableServices_storageAccounts_yourbrandstorage_name_default 'Microsoft.Storage/storageAccounts/tableServices@2023-01-01' = {
  parent: storageAccount
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}

/* ACR Roles */

@description('This is the built-in AcrPull role. See https://docs.microsoft.com/azure/role-based-access-control/built-in-roles#acrpull')
resource acrPullRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: '7f951dda-4ed3-4680-a7ca-43fe172d538d'
}

resource acrpull_store_web_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: containerRegistry
  name: guid(resourceGroup().id, storeWeb.id, acrPullRoleDefinition.id)
  properties: {
    roleDefinitionId: acrPullRoleDefinition.id
    principalId: storeWeb.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource acrpull_admin_web_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: containerRegistry
  name: guid(resourceGroup().id, adminWeb.id, acrPullRoleDefinition.id)
  properties: {
    roleDefinitionId: acrPullRoleDefinition.id
    principalId: adminWeb.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource acrpull_carts_api_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: containerRegistry
  name: guid(resourceGroup().id, cartsApi.id, acrPullRoleDefinition.id)
  properties: {
    roleDefinitionId: acrPullRoleDefinition.id
    principalId: cartsApi.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource acrpull_catalog_api_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: containerRegistry
  name: guid(resourceGroup().id, catalogApi.id, acrPullRoleDefinition.id)
  properties: {
    roleDefinitionId: acrPullRoleDefinition.id
    principalId: catalogApi.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

/* KeyVault Roles */

@description('This is the built-in KeyVault Secrets User role. See https://docs.microsoft.com/azure/role-based-access-control/built-in-roles#key-vault-secrets-user')
resource keyVaultSecretsUserRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: '4633458b-17de-408a-b874-0445c86b69e6'
}

resource kvsu_store_web_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: keyVault
  name: guid(resourceGroup().id, storeWeb.id, keyVaultSecretsUserRoleDefinition.id)
  properties: {
    roleDefinitionId: keyVaultSecretsUserRoleDefinition.id
    principalId: storeWeb.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource kvsu_admin_web_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: keyVault
  name: guid(resourceGroup().id, adminWeb.id, keyVaultSecretsUserRoleDefinition.id)
  properties: {
    roleDefinitionId: keyVaultSecretsUserRoleDefinition.id
    principalId: adminWeb.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource kvsu_carts_api_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: keyVault
  name: guid(resourceGroup().id, cartsApi.id, keyVaultSecretsUserRoleDefinition.id)
  properties: {
    roleDefinitionId: keyVaultSecretsUserRoleDefinition.id
    principalId: cartsApi.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource kvsu_catalog_api_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: keyVault
  name: guid(resourceGroup().id, catalogApi.id, keyVaultSecretsUserRoleDefinition.id)
  properties: {
    roleDefinitionId: keyVaultSecretsUserRoleDefinition.id
    principalId: catalogApi.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

/* Service Bus Roles */

@description('This is the built-in Azure Service Bus Data Owner role. See https://learn.microsoft.com/en-gb/azure/role-based-access-control/built-in-roles#azure-service-bus-data-owner')
resource azureServiceBusDataOwnerRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: '090c5cfd-751d-490a-894a-3ce6f1109419'
}

resource asbdo_store_web_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: serviceBus
  name: guid(resourceGroup().id, storeWeb.id, azureServiceBusDataOwnerRoleDefinition.id)
  properties: {
    roleDefinitionId: azureServiceBusDataOwnerRoleDefinition.id
    principalId: storeWeb.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource asbdo_admin_web_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: serviceBus
  name: guid(resourceGroup().id, adminWeb.id, azureServiceBusDataOwnerRoleDefinition.id)
  properties: {
    roleDefinitionId: azureServiceBusDataOwnerRoleDefinition.id
    principalId: adminWeb.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource asbdo_carts_api_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: serviceBus
  name: guid(resourceGroup().id, cartsApi.id, azureServiceBusDataOwnerRoleDefinition.id)
  properties: {
    roleDefinitionId: azureServiceBusDataOwnerRoleDefinition.id
    principalId: cartsApi.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource asbdo_catalog_api_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: serviceBus
  name: guid(resourceGroup().id, catalogApi.id, azureServiceBusDataOwnerRoleDefinition.id)
  properties: {
    roleDefinitionId: azureServiceBusDataOwnerRoleDefinition.id
    principalId: catalogApi.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

/* SQL Server Roles */

@description('This is the built-in Contributor role. See https://docs.microsoft.com/azure/role-based-access-control/built-in-roles#contributor')
resource contributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: 'b24988ac-6180-42a0-ab88-20f7382dd24c'
}

resource sqldb_carts_api_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: sqlServer
  name: guid(resourceGroup().id, cartsApi.id, contributorRoleDefinition.id)
  properties: {
    roleDefinitionId: contributorRoleDefinition.id
    principalId: cartsApi.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource sqldb_catalog_api_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: sqlServer
  name: guid(resourceGroup().id, catalogApi.id, contributorRoleDefinition.id)
  properties: {
    roleDefinitionId: contributorRoleDefinition.id
    principalId: catalogApi.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

/* Storage Account Roles */

@description('This is the built-in Storage Blob Data Owner role. See https://learn.microsoft.com/en-gb/azure/role-based-access-control/built-in-roles#storage-blob-data-owner')
resource storageBlobDataOwnerRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
}

resource sa_catalog_api_roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: storageAccount
  name: guid(resourceGroup().id, catalogApi.id, storageBlobDataOwnerRoleDefinition.id)
  properties: {
    roleDefinitionId: storageBlobDataOwnerRoleDefinition.id
    principalId: catalogApi.identity.principalId
    principalType: 'ServicePrincipal'
  }
}
