@description('The location where the resources will be created.')
param location string = resourceGroup().location

param containerapps_yourbrand_admin_web_name string
param containerapps_yourbrand_carts_api_name string
param containerapps_yourbrand_catalog_api_name string
param containerapps_yourbrand_store_web_name string

param managedEnvironments_yourbrand_containerappenvironment_name string
param namespaces_yourbrand_servicebus_name string
param registries_yourbrandcr_name string
param servers_yourbrand_sqlserver_name string
param storageAccounts_yourbrandstorage_name string
param vaults_yourbrand_keyvault_name string

param virtualNetworks_vnet_YourBrand_bd77_name string
var subnetName = 'backendSubnet'
var subnetRef = resourceId('Microsoft.Network/virtualNetworks/subnets', virtualNetworks_vnet_YourBrand_bd77_name, subnetName)

@secure()
param vulnerabilityAssessments_Default_storageContainerPath string
param workspaces_workspaceyourbrand8c41_name string

resource registries_yourbrandcr_name_resource 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: registries_yourbrandcr_name
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

resource vaults_yourbrand_keyvault_name_resource 'Microsoft.KeyVault/vaults@2023-02-01' = {
  location: location
  name: vaults_yourbrand_keyvault_name
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
    vaultUri: 'https://${vaults_yourbrand_keyvault_name}.vault.azure.net/'
  }
}

resource virtualNetworks_vnet_YourBrand_bd77_name_resource 'Microsoft.Network/virtualNetworks@2023-05-01' = {
  location: location
  name: virtualNetworks_vnet_YourBrand_bd77_name
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

resource workspaces_workspaceyourbrand8c41_name_resource 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  location: location
  name: workspaces_workspaceyourbrand8c41_name
  properties: {
    features: {
      enableLogAccessUsingOnlyResourcePermissions: true
    }
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
    retentionInDays: 30
    sku: {
      name: 'PerGB2018'
    }
    workspaceCapping: {
      dailyQuotaGb: -1
    }
  }
}

resource namespaces_yourbrand_servicebus_name_resource 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' = {
  location: location
  name: namespaces_yourbrand_servicebus_name
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

resource servers_yourbrand_sqlserver_name_resource 'Microsoft.Sql/servers@2023-02-01-preview' = {
  identity: {
    type: 'SystemAssigned'
  }
  kind: 'v12.0'
  location: location
  name: servers_yourbrand_sqlserver_name
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

resource storageAccounts_yourbrandstorage_name_resource 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  kind: 'StorageV2'
  location: location
  name: storageAccounts_yourbrandstorage_name
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

resource containerapps_yourbrand_admin_web_name_resource 'Microsoft.App/containerapps@2023-05-02-preview' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: containerapps_yourbrand_admin_web_name
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
          identity: 'system'
          passwordSecretRef: 'yourbrandcrazurecrio-7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
          server: 'yourbrandcr.azurecr.io'
          username: '7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
        }
      ]
      secrets: [
        {
          name: 'yourbrandcrazurecrio-7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
        }
      ]
    }
    environmentId: managedEnvironments_yourbrand_containerappenvironment_name_resource.id
    managedEnvironmentId: managedEnvironments_yourbrand_containerappenvironment_name_resource.id
    template: {
      containers: [
        {
          image: 'yourbrandcr.azurecr.io/github-action/container-app:6372877077.1'
          name: containerapps_yourbrand_admin_web_name
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

resource containerapps_yourbrand_carts_api_name_resource 'Microsoft.App/containerapps@2023-05-02-preview' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: containerapps_yourbrand_carts_api_name
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
          identity: 'system'
          passwordSecretRef: 'yourbrandcrazurecrio-7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
          server: 'yourbrandcr.azurecr.io'
          username: '7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
        }
      ]
      secrets: [
        {
          name: 'yourbrandcrazurecrio-7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
        }
      ]
    }
    environmentId: managedEnvironments_yourbrand_containerappenvironment_name_resource.id
    managedEnvironmentId: managedEnvironments_yourbrand_containerappenvironment_name_resource.id
    template: {
      containers: [
        {
          image: 'yourbrandcr.azurecr.io/github-action/container-app:6373169170.1'
          name: containerapps_yourbrand_carts_api_name
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

resource containerapps_yourbrand_catalog_api_name_resource 'Microsoft.App/containerapps@2023-05-02-preview' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: containerapps_yourbrand_catalog_api_name
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
          identity: 'system'
          passwordSecretRef: 'yourbrandcrazurecrio-7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
          server: 'yourbrandcr.azurecr.io'
          username: '7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
        }
      ]
      secrets: [
        {
          name: 'yourbrandcrazurecrio-7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
        }
      ]
    }
    environmentId: managedEnvironments_yourbrand_containerappenvironment_name_resource.id
    managedEnvironmentId: managedEnvironments_yourbrand_containerappenvironment_name_resource.id
    template: {
      containers: [
        {
          image: 'yourbrandcr.azurecr.io/github-action/container-app:6372877073.1'
          name: containerapps_yourbrand_catalog_api_name
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

resource containerapps_yourbrand_store_web_name_resource 'Microsoft.App/containerapps@2023-05-02-preview' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: containerapps_yourbrand_store_web_name
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
          identity: 'system'
          passwordSecretRef: 'yourbrandcrazurecrio-7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
          server: 'yourbrandcr.azurecr.io'
          username: '7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
        }
      ]
      secrets: [
        {
          name: 'yourbrandcrazurecrio-7c89ad91-275d-47d7-8ce4-b5eab3ba7dde'
        }
      ]
    }
    environmentId: managedEnvironments_yourbrand_containerappenvironment_name_resource.id
    managedEnvironmentId: managedEnvironments_yourbrand_containerappenvironment_name_resource.id
    template: {
      containers: [
        {
          image: 'yourbrandcr.azurecr.io/github-action/container-app:6376722172.1'
          name: containerapps_yourbrand_store_web_name
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

resource managedEnvironments_yourbrand_containerappenvironment_name_resource 'Microsoft.App/managedEnvironments@2023-05-02-preview' = {
  location: location
  name: managedEnvironments_yourbrand_containerappenvironment_name
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: 'fe3e6270-7c0a-4f37-8c42-e47566821841'
      }
    }
    customDomainConfiguration: {}
    daprConfiguration: {}
    infrastructureResourceGroup: 'ME_${managedEnvironments_yourbrand_containerappenvironment_name}_YourBrand_swedencentral'
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

resource registries_yourbrandcr_name_repositories_admin 'Microsoft.ContainerRegistry/registries/scopeMaps@2023-07-01' = {
  parent: registries_yourbrandcr_name_resource
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

resource registries_yourbrandcr_name_repositories_pull 'Microsoft.ContainerRegistry/registries/scopeMaps@2023-07-01' = {
  parent: registries_yourbrandcr_name_resource
  name: '_repositories_pull'
  properties: {
    actions: [
      'repositories/*/content/read'
    ]
    description: 'Can pull any repository of the registry'
  }
}

resource registries_yourbrandcr_name_repositories_push 'Microsoft.ContainerRegistry/registries/scopeMaps@2023-07-01' = {
  parent: registries_yourbrandcr_name_resource
  name: '_repositories_push'
  properties: {
    actions: [
      'repositories/*/content/read'
      'repositories/*/content/write'
    ]
    description: 'Can push to any repository of the registry'
  }
}

resource vaults_yourbrand_keyvault_name_yourbrand_carts_api_url 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: vaults_yourbrand_keyvault_name_resource
  location: location
  name: 'yourbrand-carts-api-url'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource vaults_yourbrand_keyvault_name_yourbrand_carts_db_connectionstring 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: vaults_yourbrand_keyvault_name_resource
  location: location
  name: 'yourbrand-carts-db-connectionstring'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource vaults_yourbrand_keyvault_name_yourbrand_catalog_api_url 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: vaults_yourbrand_keyvault_name_resource
  location: location
  name: 'yourbrand-catalog-api-url'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource vaults_yourbrand_keyvault_name_yourbrand_catalog_db_connectionstring 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: vaults_yourbrand_keyvault_name_resource
  location: location
  name: 'yourbrand-catalog-db-connectionstring'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource vaults_yourbrand_keyvault_name_yourbrand_servicebus_connectionstring 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: vaults_yourbrand_keyvault_name_resource
  location: location
  name: 'yourbrand-servicebus-connectionstring'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource virtualNetworks_vnet_YourBrand_bd77_name_infra_subnet 'Microsoft.Network/virtualNetworks/subnets@2023-05-01' = {
  parent: virtualNetworks_vnet_YourBrand_bd77_name_resource
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
  dependsOn: [
    virtualNetworks_vnet_YourBrand_bd77_name_resource
  ]
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_General_AlphabeticallySortedComputers 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_General|AlphabeticallySortedComputers'
  properties: {
    category: 'General Exploration'
    displayName: 'All Computers with their most recent data'
    query: 'search not(ObjectName == "Advisor Metrics" or ObjectName == "ManagedSpace") | summarize AggregatedValue = max(TimeGenerated) by Computer | limit 500000 | sort by Computer asc\r\n// Oql: NOT(ObjectName="Advisor Metrics" OR ObjectName=ManagedSpace) | measure max(TimeGenerated) by Computer | top 500000 | Sort Computer // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_General_dataPointsPerManagementGroup 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_General|dataPointsPerManagementGroup'
  properties: {
    category: 'General Exploration'
    displayName: 'Which Management Group is generating the most data points?'
    query: 'search * | summarize AggregatedValue = count() by ManagementGroupName\r\n// Oql: * | Measure count() by ManagementGroupName // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_General_dataTypeDistribution 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_General|dataTypeDistribution'
  properties: {
    category: 'General Exploration'
    displayName: 'Distribution of data Types'
    query: 'search * | extend Type = $table | summarize AggregatedValue = count() by Type\r\n// Oql: * | Measure count() by Type // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_General_StaleComputers 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_General|StaleComputers'
  properties: {
    category: 'General Exploration'
    displayName: 'Stale Computers (data older than 24 hours)'
    query: 'search not(ObjectName == "Advisor Metrics" or ObjectName == "ManagedSpace") | summarize lastdata = max(TimeGenerated) by Computer | limit 500000 | where lastdata < ago(24h)\r\n// Oql: NOT(ObjectName="Advisor Metrics" OR ObjectName=ManagedSpace) | measure max(TimeGenerated) as lastdata by Computer | top 500000 | where lastdata < NOW-24HOURS // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_AllEvents 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|AllEvents'
  properties: {
    category: 'Log Management'
    displayName: 'All Events'
    query: 'Event | sort by TimeGenerated desc\r\n// Oql: Type=Event // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_AllSyslog 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|AllSyslog'
  properties: {
    category: 'Log Management'
    displayName: 'All Syslogs'
    query: 'Syslog | sort by TimeGenerated desc\r\n// Oql: Type=Syslog // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_AllSyslogByFacility 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|AllSyslogByFacility'
  properties: {
    category: 'Log Management'
    displayName: 'All Syslog Records grouped by Facility'
    query: 'Syslog | summarize AggregatedValue = count() by Facility\r\n// Oql: Type=Syslog | Measure count() by Facility // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_AllSyslogByProcess 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|AllSyslogByProcessName'
  properties: {
    category: 'Log Management'
    displayName: 'All Syslog Records grouped by ProcessName'
    query: 'Syslog | summarize AggregatedValue = count() by ProcessName\r\n// Oql: Type=Syslog | Measure count() by ProcessName // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_AllSyslogsWithErrors 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|AllSyslogsWithErrors'
  properties: {
    category: 'Log Management'
    displayName: 'All Syslog Records with Errors'
    query: 'Syslog | where SeverityLevel == "error" | sort by TimeGenerated desc\r\n// Oql: Type=Syslog SeverityLevel=error // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_AverageHTTPRequestTimeByClientIPAddress 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|AverageHTTPRequestTimeByClientIPAddress'
  properties: {
    category: 'Log Management'
    displayName: 'Average HTTP Request time by Client IP Address'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = avg(TimeTaken) by cIP\r\n// Oql: Type=W3CIISLog | Measure Avg(TimeTaken) by cIP // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_AverageHTTPRequestTimeHTTPMethod 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|AverageHTTPRequestTimeHTTPMethod'
  properties: {
    category: 'Log Management'
    displayName: 'Average HTTP Request time by HTTP Method'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = avg(TimeTaken) by csMethod\r\n// Oql: Type=W3CIISLog | Measure Avg(TimeTaken) by csMethod // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_CountIISLogEntriesClientIPAddress 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|CountIISLogEntriesClientIPAddress'
  properties: {
    category: 'Log Management'
    displayName: 'Count of IIS Log Entries by Client IP Address'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by cIP\r\n// Oql: Type=W3CIISLog | Measure count() by cIP // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_CountIISLogEntriesHTTPRequestMethod 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|CountIISLogEntriesHTTPRequestMethod'
  properties: {
    category: 'Log Management'
    displayName: 'Count of IIS Log Entries by HTTP Request Method'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csMethod\r\n// Oql: Type=W3CIISLog | Measure count() by csMethod // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_CountIISLogEntriesHTTPUserAgent 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|CountIISLogEntriesHTTPUserAgent'
  properties: {
    category: 'Log Management'
    displayName: 'Count of IIS Log Entries by HTTP User Agent'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csUserAgent\r\n// Oql: Type=W3CIISLog | Measure count() by csUserAgent // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_CountOfIISLogEntriesByHostRequestedByClient 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|CountOfIISLogEntriesByHostRequestedByClient'
  properties: {
    category: 'Log Management'
    displayName: 'Count of IIS Log Entries by Host requested by client'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csHost\r\n// Oql: Type=W3CIISLog | Measure count() by csHost // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_CountOfIISLogEntriesByURLForHost 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|CountOfIISLogEntriesByURLForHost'
  properties: {
    category: 'Log Management'
    displayName: 'Count of IIS Log Entries by URL for the host "www.contoso.com" (replace with your own)'
    query: 'search csHost == "www.contoso.com" | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csUriStem\r\n// Oql: Type=W3CIISLog csHost="www.contoso.com" | Measure count() by csUriStem // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_CountOfIISLogEntriesByURLRequestedByClient 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|CountOfIISLogEntriesByURLRequestedByClient'
  properties: {
    category: 'Log Management'
    displayName: 'Count of IIS Log Entries by URL requested by client (without query strings)'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csUriStem\r\n// Oql: Type=W3CIISLog | Measure count() by csUriStem // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_CountOfWarningEvents 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|CountOfWarningEvents'
  properties: {
    category: 'Log Management'
    displayName: 'Count of Events with level "Warning" grouped by Event ID'
    query: 'Event | where EventLevelName == "warning" | summarize AggregatedValue = count() by EventID\r\n// Oql: Type=Event EventLevelName=warning | Measure count() by EventID // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_DisplayBreakdownRespondCodes 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|DisplayBreakdownRespondCodes'
  properties: {
    category: 'Log Management'
    displayName: 'Shows breakdown of response codes'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by scStatus\r\n// Oql: Type=W3CIISLog | Measure count() by scStatus // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_EventsByEventLog 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|EventsByEventLog'
  properties: {
    category: 'Log Management'
    displayName: 'Count of Events grouped by Event Log'
    query: 'Event | summarize AggregatedValue = count() by EventLog\r\n// Oql: Type=Event | Measure count() by EventLog // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_EventsByEventsID 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|EventsByEventsID'
  properties: {
    category: 'Log Management'
    displayName: 'Count of Events grouped by Event ID'
    query: 'Event | summarize AggregatedValue = count() by EventID\r\n// Oql: Type=Event | Measure count() by EventID // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_EventsByEventSource 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|EventsByEventSource'
  properties: {
    category: 'Log Management'
    displayName: 'Count of Events grouped by Event Source'
    query: 'Event | summarize AggregatedValue = count() by Source\r\n// Oql: Type=Event | Measure count() by Source // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_EventsInOMBetween2000to3000 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|EventsInOMBetween2000to3000'
  properties: {
    category: 'Log Management'
    displayName: 'Events in the Operations Manager Event Log whose Event ID is in the range between 2000 and 3000'
    query: 'Event | where EventLog == "Operations Manager" and EventID >= 2000 and EventID <= 3000 | sort by TimeGenerated desc\r\n// Oql: Type=Event EventLog="Operations Manager" EventID:[2000..3000] // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_EventsWithStartedinEventID 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|EventsWithStartedinEventID'
  properties: {
    category: 'Log Management'
    displayName: 'Count of Events containing the word "started" grouped by EventID'
    query: 'search in (Event) "started" | summarize AggregatedValue = count() by EventID\r\n// Oql: Type=Event "started" | Measure count() by EventID // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_FindMaximumTimeTakenForEachPage 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|FindMaximumTimeTakenForEachPage'
  properties: {
    category: 'Log Management'
    displayName: 'Find the maximum time taken for each page'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = max(TimeTaken) by csUriStem\r\n// Oql: Type=W3CIISLog | Measure Max(TimeTaken) by csUriStem // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_IISLogEntriesForClientIP 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|IISLogEntriesForClientIP'
  properties: {
    category: 'Log Management'
    displayName: 'IIS Log Entries for a specific client IP Address (replace with your own)'
    query: 'search cIP == "192.168.0.1" | extend Type = $table | where Type == W3CIISLog | sort by TimeGenerated desc | project csUriStem, scBytes, csBytes, TimeTaken, scStatus\r\n// Oql: Type=W3CIISLog cIP="192.168.0.1" | Select csUriStem,scBytes,csBytes,TimeTaken,scStatus // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_ListAllIISLogEntries 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|ListAllIISLogEntries'
  properties: {
    category: 'Log Management'
    displayName: 'All IIS Log Entries'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | sort by TimeGenerated desc\r\n// Oql: Type=W3CIISLog // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_NoOfConnectionsToOMSDKService 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|NoOfConnectionsToOMSDKService'
  properties: {
    category: 'Log Management'
    displayName: 'How many connections to Operations Manager\'s SDK service by day'
    query: 'Event | where EventID == 26328 and EventLog == "Operations Manager" | summarize AggregatedValue = count() by bin(TimeGenerated, 1d) | sort by TimeGenerated desc\r\n// Oql: Type=Event EventID=26328 EventLog="Operations Manager" | Measure count() interval 1DAY // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_ServerRestartTime 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|ServerRestartTime'
  properties: {
    category: 'Log Management'
    displayName: 'When did my servers initiate restart?'
    query: 'search in (Event) "shutdown" and EventLog == "System" and Source == "User32" and EventID == 1074 | sort by TimeGenerated desc | project TimeGenerated, Computer\r\n// Oql: shutdown Type=Event EventLog=System Source=User32 EventID=1074 | Select TimeGenerated,Computer // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_Show404PagesList 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|Show404PagesList'
  properties: {
    category: 'Log Management'
    displayName: 'Shows which pages people are getting a 404 for'
    query: 'search scStatus == 404 | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csUriStem\r\n// Oql: Type=W3CIISLog scStatus=404 | Measure count() by csUriStem // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_ShowServersThrowingInternalServerError 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|ShowServersThrowingInternalServerError'
  properties: {
    category: 'Log Management'
    displayName: 'Shows servers that are throwing internal server error'
    query: 'search scStatus == 500 | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by sComputerName\r\n// Oql: Type=W3CIISLog scStatus=500 | Measure count() by sComputerName // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_TotalBytesReceivedByEachAzureRoleInstance 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|TotalBytesReceivedByEachAzureRoleInstance'
  properties: {
    category: 'Log Management'
    displayName: 'Total Bytes received by each Azure Role Instance'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = sum(csBytes) by RoleInstance\r\n// Oql: Type=W3CIISLog | Measure Sum(csBytes) by RoleInstance // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_TotalBytesReceivedByEachIISComputer 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|TotalBytesReceivedByEachIISComputer'
  properties: {
    category: 'Log Management'
    displayName: 'Total Bytes received by each IIS Computer'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = sum(csBytes) by Computer | limit 500000\r\n// Oql: Type=W3CIISLog | Measure Sum(csBytes) by Computer | top 500000 // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_TotalBytesRespondedToClientsByClientIPAddress 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|TotalBytesRespondedToClientsByClientIPAddress'
  properties: {
    category: 'Log Management'
    displayName: 'Total Bytes responded back to clients by Client IP Address'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = sum(scBytes) by cIP\r\n// Oql: Type=W3CIISLog | Measure Sum(scBytes) by cIP // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_TotalBytesRespondedToClientsByEachIISServerIPAddress 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|TotalBytesRespondedToClientsByEachIISServerIPAddress'
  properties: {
    category: 'Log Management'
    displayName: 'Total Bytes responded back to clients by each IIS ServerIP Address'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = sum(scBytes) by sIP\r\n// Oql: Type=W3CIISLog | Measure Sum(scBytes) by sIP // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_TotalBytesSentByClientIPAddress 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|TotalBytesSentByClientIPAddress'
  properties: {
    category: 'Log Management'
    displayName: 'Total Bytes sent by Client IP Address'
    query: 'search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = sum(csBytes) by cIP\r\n// Oql: Type=W3CIISLog | Measure Sum(csBytes) by cIP // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_WarningEvents 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|WarningEvents'
  properties: {
    category: 'Log Management'
    displayName: 'All Events with level "Warning"'
    query: 'Event | where EventLevelName == "warning" | sort by TimeGenerated desc\r\n// Oql: Type=Event EventLevelName=warning // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_WindowsFireawallPolicySettingsChanged 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|WindowsFireawallPolicySettingsChanged'
  properties: {
    category: 'Log Management'
    displayName: 'Windows Firewall Policy settings have changed'
    query: 'Event | where EventLog == "Microsoft-Windows-Windows Firewall With Advanced Security/Firewall" and EventID == 2008 | sort by TimeGenerated desc\r\n// Oql: Type=Event EventLog="Microsoft-Windows-Windows Firewall With Advanced Security/Firewall" EventID=2008 // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogManagement_workspaces_workspaceyourbrand8c41_name_LogManagement_WindowsFireawallPolicySettingsChangedByMachines 'Microsoft.OperationalInsights/workspaces/savedSearches@2020-08-01' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogManagement(${workspaces_workspaceyourbrand8c41_name})_LogManagement|WindowsFireawallPolicySettingsChangedByMachines'
  properties: {
    category: 'Log Management'
    displayName: 'On which machines and how many times have Windows Firewall Policy settings changed'
    query: 'Event | where EventLog == "Microsoft-Windows-Windows Firewall With Advanced Security/Firewall" and EventID == 2008 | summarize AggregatedValue = count() by Computer | limit 500000\r\n// Oql: Type=Event EventLog="Microsoft-Windows-Windows Firewall With Advanced Security/Firewall" EventID=2008 | measure count() by Computer | top 500000 // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122'
    version: 2
  }
}

resource workspaces_workspaceyourbrand8c41_name_AACAudit 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AACAudit'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AACAudit'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AACHttpRequest 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AACHttpRequest'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AACHttpRequest'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADB2CRequestLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADB2CRequestLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADB2CRequestLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADDomainServicesAccountLogon 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADDomainServicesAccountLogon'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADDomainServicesAccountLogon'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADDomainServicesAccountManagement 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADDomainServicesAccountManagement'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADDomainServicesAccountManagement'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADDomainServicesDirectoryServiceAccess 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADDomainServicesDirectoryServiceAccess'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADDomainServicesDirectoryServiceAccess'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADDomainServicesDNSAuditsDynamicUpdates 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADDomainServicesDNSAuditsDynamicUpdates'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADDomainServicesDNSAuditsDynamicUpdates'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADDomainServicesDNSAuditsGeneral 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADDomainServicesDNSAuditsGeneral'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADDomainServicesDNSAuditsGeneral'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADDomainServicesLogonLogoff 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADDomainServicesLogonLogoff'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADDomainServicesLogonLogoff'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADDomainServicesPolicyChange 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADDomainServicesPolicyChange'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADDomainServicesPolicyChange'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADDomainServicesPrivilegeUse 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADDomainServicesPrivilegeUse'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADDomainServicesPrivilegeUse'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADManagedIdentitySignInLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADManagedIdentitySignInLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADManagedIdentitySignInLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADNonInteractiveUserSignInLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADNonInteractiveUserSignInLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADNonInteractiveUserSignInLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADProvisioningLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADProvisioningLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADProvisioningLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADRiskyServicePrincipals 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADRiskyServicePrincipals'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADRiskyServicePrincipals'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADRiskyUsers 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADRiskyUsers'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADRiskyUsers'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADServicePrincipalRiskEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADServicePrincipalRiskEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADServicePrincipalRiskEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADServicePrincipalSignInLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADServicePrincipalSignInLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADServicePrincipalSignInLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AADUserRiskEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AADUserRiskEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AADUserRiskEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ABSBotRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ABSBotRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ABSBotRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACICollaborationAudit 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACICollaborationAudit'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACICollaborationAudit'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACRConnectedClientList 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACRConnectedClientList'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACRConnectedClientList'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSAuthIncomingOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSAuthIncomingOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSAuthIncomingOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSBillingUsage 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSBillingUsage'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSBillingUsage'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSCallAutomationIncomingOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSCallAutomationIncomingOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSCallAutomationIncomingOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSCallAutomationMediaSummary 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSCallAutomationMediaSummary'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSCallAutomationMediaSummary'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSCallDiagnostics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSCallDiagnostics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSCallDiagnostics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSCallRecordingIncomingOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSCallRecordingIncomingOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSCallRecordingIncomingOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSCallRecordingSummary 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSCallRecordingSummary'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSCallRecordingSummary'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSCallSummary 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSCallSummary'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSCallSummary'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSCallSurvey 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSCallSurvey'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSCallSurvey'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSChatIncomingOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSChatIncomingOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSChatIncomingOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSEmailSendMailOperational 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSEmailSendMailOperational'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSEmailSendMailOperational'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSEmailStatusUpdateOperational 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSEmailStatusUpdateOperational'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSEmailStatusUpdateOperational'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSEmailUserEngagementOperational 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSEmailUserEngagementOperational'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSEmailUserEngagementOperational'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSJobRouterIncomingOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSJobRouterIncomingOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSJobRouterIncomingOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSNetworkTraversalDiagnostics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSNetworkTraversalDiagnostics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSNetworkTraversalDiagnostics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSNetworkTraversalIncomingOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSNetworkTraversalIncomingOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSNetworkTraversalIncomingOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSRoomsIncomingOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSRoomsIncomingOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSRoomsIncomingOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ACSSMSIncomingOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ACSSMSIncomingOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ACSSMSIncomingOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AddonAzureBackupAlerts 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AddonAzureBackupAlerts'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AddonAzureBackupAlerts'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AddonAzureBackupJobs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AddonAzureBackupJobs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AddonAzureBackupJobs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AddonAzureBackupPolicy 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AddonAzureBackupPolicy'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AddonAzureBackupPolicy'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AddonAzureBackupProtectedInstance 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AddonAzureBackupProtectedInstance'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AddonAzureBackupProtectedInstance'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AddonAzureBackupStorage 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AddonAzureBackupStorage'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AddonAzureBackupStorage'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFActivityRun 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFActivityRun'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFActivityRun'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFAirflowSchedulerLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFAirflowSchedulerLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFAirflowSchedulerLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFAirflowTaskLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFAirflowTaskLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFAirflowTaskLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFAirflowWebLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFAirflowWebLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFAirflowWebLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFAirflowWorkerLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFAirflowWorkerLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFAirflowWorkerLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFPipelineRun 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFPipelineRun'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFPipelineRun'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFSandboxActivityRun 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFSandboxActivityRun'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFSandboxActivityRun'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFSandboxPipelineRun 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFSandboxPipelineRun'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFSandboxPipelineRun'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFSSignInLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFSSignInLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFSSignInLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFSSISIntegrationRuntimeLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFSSISIntegrationRuntimeLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFSSISIntegrationRuntimeLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFSSISPackageEventMessageContext 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFSSISPackageEventMessageContext'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFSSISPackageEventMessageContext'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFSSISPackageEventMessages 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFSSISPackageEventMessages'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFSSISPackageEventMessages'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFSSISPackageExecutableStatistics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFSSISPackageExecutableStatistics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFSSISPackageExecutableStatistics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFSSISPackageExecutionComponentPhases 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFSSISPackageExecutionComponentPhases'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFSSISPackageExecutionComponentPhases'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFSSISPackageExecutionDataStatistics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFSSISPackageExecutionDataStatistics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFSSISPackageExecutionDataStatistics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADFTriggerRun 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADFTriggerRun'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADFTriggerRun'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADPAudit 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADPAudit'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADPAudit'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADPDiagnostics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADPDiagnostics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADPDiagnostics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADPRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADPRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADPRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADSecurityAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADSecurityAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADSecurityAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADTDataHistoryOperation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADTDataHistoryOperation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADTDataHistoryOperation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADTDigitalTwinsOperation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADTDigitalTwinsOperation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADTDigitalTwinsOperation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADTEventRoutesOperation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADTEventRoutesOperation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADTEventRoutesOperation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADTModelsOperation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADTModelsOperation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADTModelsOperation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADTQueryOperation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADTQueryOperation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADTQueryOperation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADXCommand 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADXCommand'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADXCommand'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADXIngestionBatching 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADXIngestionBatching'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADXIngestionBatching'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADXJournal 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADXJournal'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADXJournal'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADXQuery 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADXQuery'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADXQuery'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADXTableDetails 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADXTableDetails'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADXTableDetails'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ADXTableUsageStatistics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ADXTableUsageStatistics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ADXTableUsageStatistics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AegDataPlaneRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AegDataPlaneRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AegDataPlaneRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AegDeliveryFailureLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AegDeliveryFailureLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AegDeliveryFailureLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AegPublishFailureLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AegPublishFailureLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AegPublishFailureLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AEWAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AEWAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AEWAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AEWComputePipelinesLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AEWComputePipelinesLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AEWComputePipelinesLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AFSAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AFSAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AFSAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AgriFoodApplicationAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AgriFoodApplicationAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AgriFoodApplicationAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AgriFoodFarmManagementLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AgriFoodFarmManagementLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AgriFoodFarmManagementLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AgriFoodFarmOperationLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AgriFoodFarmOperationLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AgriFoodFarmOperationLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AgriFoodInsightLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AgriFoodInsightLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AgriFoodInsightLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AgriFoodJobProcessedLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AgriFoodJobProcessedLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AgriFoodJobProcessedLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AgriFoodModelInferenceLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AgriFoodModelInferenceLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AgriFoodModelInferenceLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AgriFoodProviderAuthLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AgriFoodProviderAuthLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AgriFoodProviderAuthLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AgriFoodSatelliteLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AgriFoodSatelliteLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AgriFoodSatelliteLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AgriFoodSensorManagementLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AgriFoodSensorManagementLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AgriFoodSensorManagementLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AgriFoodWeatherLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AgriFoodWeatherLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AgriFoodWeatherLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AGSGrafanaLoginEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AGSGrafanaLoginEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AGSGrafanaLoginEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AHDSDicomAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AHDSDicomAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AHDSDicomAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AHDSDicomDiagnosticLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AHDSDicomDiagnosticLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AHDSDicomDiagnosticLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AHDSMedTechDiagnosticLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AHDSMedTechDiagnosticLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AHDSMedTechDiagnosticLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AirflowDagProcessingLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AirflowDagProcessingLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AirflowDagProcessingLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AKSAudit 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AKSAudit'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AKSAudit'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AKSAuditAdmin 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AKSAuditAdmin'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AKSAuditAdmin'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AKSControlPlane 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AKSControlPlane'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AKSControlPlane'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_Alert 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'Alert'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'Alert'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlComputeClusterEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlComputeClusterEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlComputeClusterEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlComputeClusterNodeEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlComputeClusterNodeEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlComputeClusterNodeEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlComputeCpuGpuUtilization 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlComputeCpuGpuUtilization'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlComputeCpuGpuUtilization'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlComputeInstanceEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlComputeInstanceEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlComputeInstanceEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlComputeJobEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlComputeJobEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlComputeJobEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlDataLabelEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlDataLabelEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlDataLabelEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlDataSetEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlDataSetEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlDataSetEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlDataStoreEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlDataStoreEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlDataStoreEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlDeploymentEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlDeploymentEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlDeploymentEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlEnvironmentEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlEnvironmentEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlEnvironmentEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlInferencingEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlInferencingEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlInferencingEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlModelsEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlModelsEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlModelsEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlOnlineEndpointConsoleLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlOnlineEndpointConsoleLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlOnlineEndpointConsoleLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlOnlineEndpointEventLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlOnlineEndpointEventLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlOnlineEndpointEventLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlOnlineEndpointTrafficLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlOnlineEndpointTrafficLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlOnlineEndpointTrafficLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlPipelineEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlPipelineEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlPipelineEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlRegistryReadEventsLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlRegistryReadEventsLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlRegistryReadEventsLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlRegistryWriteEventsLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlRegistryWriteEventsLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlRegistryWriteEventsLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlRunEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlRunEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlRunEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AmlRunStatusChangedEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AmlRunStatusChangedEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AmlRunStatusChangedEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AMSKeyDeliveryRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AMSKeyDeliveryRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AMSKeyDeliveryRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AMSLiveEventOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AMSLiveEventOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AMSLiveEventOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AMSMediaAccountHealth 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AMSMediaAccountHealth'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AMSMediaAccountHealth'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AMSStreamingEndpointRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AMSStreamingEndpointRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AMSStreamingEndpointRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ANFFileAccess 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ANFFileAccess'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ANFFileAccess'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ApiManagementGatewayLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ApiManagementGatewayLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ApiManagementGatewayLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ApiManagementWebSocketConnectionLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ApiManagementWebSocketConnectionLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ApiManagementWebSocketConnectionLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppAvailabilityResults 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppAvailabilityResults'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AppAvailabilityResults'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppBrowserTimings 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppBrowserTimings'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AppBrowserTimings'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppCenterError 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppCenterError'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppCenterError'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppDependencies 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppDependencies'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AppDependencies'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppEnvSpringAppConsoleLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppEnvSpringAppConsoleLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppEnvSpringAppConsoleLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AppEvents'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppExceptions 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppExceptions'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AppExceptions'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppMetrics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppMetrics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AppMetrics'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppPageViews 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppPageViews'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AppPageViews'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppPerformanceCounters 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppPerformanceCounters'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AppPerformanceCounters'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppPlatformBuildLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppPlatformBuildLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppPlatformBuildLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppPlatformContainerEventLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppPlatformContainerEventLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppPlatformContainerEventLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppPlatformIngressLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppPlatformIngressLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppPlatformIngressLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppPlatformLogsforSpring 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppPlatformLogsforSpring'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppPlatformLogsforSpring'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppPlatformSystemLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppPlatformSystemLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppPlatformSystemLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AppRequests'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppServiceAntivirusScanAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppServiceAntivirusScanAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppServiceAntivirusScanAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppServiceAppLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppServiceAppLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppServiceAppLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppServiceAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppServiceAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppServiceAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppServiceConsoleLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppServiceConsoleLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppServiceConsoleLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppServiceEnvironmentPlatformLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppServiceEnvironmentPlatformLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppServiceEnvironmentPlatformLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppServiceFileAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppServiceFileAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppServiceFileAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppServiceHTTPLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppServiceHTTPLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppServiceHTTPLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppServiceIPSecAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppServiceIPSecAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppServiceIPSecAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppServicePlatformLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppServicePlatformLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppServicePlatformLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppServiceServerlessSecurityPluginData 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppServiceServerlessSecurityPluginData'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AppServiceServerlessSecurityPluginData'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppSystemEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppSystemEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AppSystemEvents'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_AppTraces 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AppTraces'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AppTraces'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_ASCAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ASCAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ASCAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ASCDeviceEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ASCDeviceEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ASCDeviceEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ASimAuditEventLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ASimAuditEventLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ASimAuditEventLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ASimAuthenticationEventLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ASimAuthenticationEventLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ASimAuthenticationEventLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ASimProcessEventLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ASimProcessEventLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ASimProcessEventLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ASRJobs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ASRJobs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ASRJobs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ASRReplicatedItems 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ASRReplicatedItems'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ASRReplicatedItems'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ATCExpressRouteCircuitIpfix 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ATCExpressRouteCircuitIpfix'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ATCExpressRouteCircuitIpfix'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AUIEventsAudit 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AUIEventsAudit'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AUIEventsAudit'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AUIEventsOperational 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AUIEventsOperational'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AUIEventsOperational'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AutoscaleEvaluationsLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AutoscaleEvaluationsLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AutoscaleEvaluationsLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AutoscaleScaleActionsLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AutoscaleScaleActionsLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AutoscaleScaleActionsLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AVNMNetworkGroupMembershipChange 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AVNMNetworkGroupMembershipChange'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AVNMNetworkGroupMembershipChange'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AVNMRuleCollectionChange 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AVNMRuleCollectionChange'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AVNMRuleCollectionChange'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AVSSyslog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AVSSyslog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AVSSyslog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWApplicationRule 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWApplicationRule'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWApplicationRule'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWApplicationRuleAggregation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWApplicationRuleAggregation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWApplicationRuleAggregation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWDnsQuery 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWDnsQuery'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWDnsQuery'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWFatFlow 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWFatFlow'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWFatFlow'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWFlowTrace 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWFlowTrace'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWFlowTrace'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWIdpsSignature 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWIdpsSignature'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWIdpsSignature'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWInternalFqdnResolutionFailure 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWInternalFqdnResolutionFailure'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWInternalFqdnResolutionFailure'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWNatRule 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWNatRule'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWNatRule'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWNatRuleAggregation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWNatRuleAggregation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWNatRuleAggregation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWNetworkRule 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWNetworkRule'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWNetworkRule'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWNetworkRuleAggregation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWNetworkRuleAggregation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWNetworkRuleAggregation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZFWThreatIntel 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZFWThreatIntel'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZFWThreatIntel'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZKVAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZKVAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZKVAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZKVPolicyEvaluationDetailsLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZKVPolicyEvaluationDetailsLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZKVPolicyEvaluationDetailsLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZMSApplicationMetricLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZMSApplicationMetricLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZMSApplicationMetricLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZMSArchiveLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZMSArchiveLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZMSArchiveLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZMSAutoscaleLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZMSAutoscaleLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZMSAutoscaleLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZMSCustomerManagedKeyUserLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZMSCustomerManagedKeyUserLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZMSCustomerManagedKeyUserLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZMSHybridConnectionsEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZMSHybridConnectionsEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZMSHybridConnectionsEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZMSKafkaCoordinatorLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZMSKafkaCoordinatorLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZMSKafkaCoordinatorLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZMSKafkaUserErrorLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZMSKafkaUserErrorLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZMSKafkaUserErrorLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZMSOperationalLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZMSOperationalLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZMSOperationalLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZMSRunTimeAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZMSRunTimeAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZMSRunTimeAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AZMSVnetConnectionEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AZMSVnetConnectionEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AZMSVnetConnectionEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AzureActivity 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AzureActivity'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'AzureActivity'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_AzureActivityV2 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AzureActivityV2'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AzureActivityV2'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AzureAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AzureAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AzureAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AzureAttestationDiagnostics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AzureAttestationDiagnostics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AzureAttestationDiagnostics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AzureCloudHsmAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AzureCloudHsmAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AzureCloudHsmAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AzureDevOpsAuditing 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AzureDevOpsAuditing'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AzureDevOpsAuditing'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AzureLoadTestingOperation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AzureLoadTestingOperation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AzureLoadTestingOperation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_AzureMetrics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'AzureMetrics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'AzureMetrics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_BlockchainApplicationLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'BlockchainApplicationLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'BlockchainApplicationLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_BlockchainProxyLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'BlockchainProxyLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'BlockchainProxyLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CassandraAudit 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CassandraAudit'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CassandraAudit'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CassandraLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CassandraLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CassandraLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CCFApplicationLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CCFApplicationLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CCFApplicationLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CDBCassandraRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CDBCassandraRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CDBCassandraRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CDBControlPlaneRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CDBControlPlaneRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CDBControlPlaneRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CDBDataPlaneRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CDBDataPlaneRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CDBDataPlaneRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CDBGremlinRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CDBGremlinRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CDBGremlinRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CDBMongoRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CDBMongoRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CDBMongoRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CDBPartitionKeyRUConsumption 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CDBPartitionKeyRUConsumption'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CDBPartitionKeyRUConsumption'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CDBPartitionKeyStatistics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CDBPartitionKeyStatistics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CDBPartitionKeyStatistics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CDBQueryRuntimeStatistics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CDBQueryRuntimeStatistics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CDBQueryRuntimeStatistics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ChaosStudioExperimentEventLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ChaosStudioExperimentEventLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ChaosStudioExperimentEventLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CHSMManagementAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CHSMManagementAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CHSMManagementAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CIEventsAudit 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CIEventsAudit'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CIEventsAudit'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CIEventsOperational 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CIEventsOperational'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CIEventsOperational'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ComputerGroup 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ComputerGroup'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ComputerGroup'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerAppConsoleLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerAppConsoleLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerAppConsoleLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerAppConsoleLogs_CL 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerAppConsoleLogs_CL'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      columns: [
        {
          name: '_timestamp_d'
          type: 'real'
        }
        {
          name: 'ContainerGroupName_s'
          type: 'string'
        }
        {
          name: 'ContainerName_s'
          type: 'string'
        }
        {
          name: 'ContainerGroupId_g'
          type: 'guid'
        }
        {
          name: 'RevisionName_s'
          type: 'string'
        }
        {
          name: 'ContainerAppName_s'
          type: 'string'
        }
        {
          name: 'Log_s'
          type: 'string'
        }
        {
          name: 'EnvironmentName_s'
          type: 'string'
        }
      ]
      name: 'ContainerAppConsoleLogs_CL'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerAppSystemLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerAppSystemLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerAppSystemLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerAppSystemLogs_CL 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerAppSystemLogs_CL'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      columns: [
        {
          name: 'Error_s'
          type: 'string'
        }
        {
          name: 'time_t'
          type: 'datetime'
        }
        {
          name: '_timestamp_d'
          type: 'real'
        }
        {
          name: 'Count_d'
          type: 'real'
        }
        {
          name: 'JobName_s'
          type: 'string'
        }
        {
          name: 'EventSource_s'
          type: 'string'
        }
        {
          name: 'ContainerAppName_s'
          type: 'string'
        }
        {
          name: 'ReplicaName_s'
          type: 'string'
        }
        {
          name: 'TimeStamp_s'
          type: 'string'
        }
        {
          name: 'Internal_b'
          type: 'boolean'
        }
        {
          name: 'time_s'
          type: 'string'
        }
        {
          name: 'Type_s'
          type: 'string'
        }
        {
          name: 'EnvironmentName_s'
          type: 'string'
        }
        {
          name: 'Reason_s'
          type: 'string'
        }
        {
          name: 'Log_s'
          type: 'string'
        }
        {
          name: 'ExecutionName_s'
          type: 'string'
        }
        {
          name: 'RevisionName_s'
          type: 'string'
        }
        {
          name: 'Level'
          type: 'string'
        }
      ]
      name: 'ContainerAppSystemLogs_CL'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerImageInventory 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerImageInventory'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerImageInventory'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerInstanceLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerInstanceLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerInstanceLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerInventory 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerInventory'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerInventory'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerLogV2 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerLogV2'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerLogV2'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerNodeInventory 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerNodeInventory'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerNodeInventory'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerRegistryLoginEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerRegistryLoginEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerRegistryLoginEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerRegistryRepositoryEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerRegistryRepositoryEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerRegistryRepositoryEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ContainerServiceLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ContainerServiceLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ContainerServiceLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_CoreAzureBackup 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'CoreAzureBackup'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'CoreAzureBackup'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksAccounts 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksAccounts'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksAccounts'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksCapsule8Dataplane 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksCapsule8Dataplane'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksCapsule8Dataplane'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksClamAVScan 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksClamAVScan'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksClamAVScan'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksClusterLibraries 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksClusterLibraries'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksClusterLibraries'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksClusters 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksClusters'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksClusters'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksDatabricksSQL 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksDatabricksSQL'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksDatabricksSQL'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksDBFS 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksDBFS'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksDBFS'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksDeltaPipelines 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksDeltaPipelines'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksDeltaPipelines'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksFeatureStore 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksFeatureStore'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksFeatureStore'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksGenie 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksGenie'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksGenie'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksGitCredentials 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksGitCredentials'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksGitCredentials'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksGlobalInitScripts 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksGlobalInitScripts'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksGlobalInitScripts'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksIAMRole 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksIAMRole'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksIAMRole'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksInstancePools 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksInstancePools'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksInstancePools'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksJobs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksJobs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksJobs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksMLflowAcledArtifact 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksMLflowAcledArtifact'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksMLflowAcledArtifact'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksMLflowExperiment 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksMLflowExperiment'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksMLflowExperiment'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksModelRegistry 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksModelRegistry'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksModelRegistry'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksNotebook 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksNotebook'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksNotebook'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksPartnerHub 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksPartnerHub'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksPartnerHub'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksRemoteHistoryService 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksRemoteHistoryService'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksRemoteHistoryService'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksRepos 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksRepos'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksRepos'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksSecrets 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksSecrets'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksSecrets'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksServerlessRealTimeInference 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksServerlessRealTimeInference'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksServerlessRealTimeInference'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksSQL 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksSQL'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksSQL'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksSQLPermissions 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksSQLPermissions'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksSQLPermissions'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksSSH 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksSSH'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksSSH'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksUnityCatalog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksUnityCatalog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksUnityCatalog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksWebTerminal 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksWebTerminal'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksWebTerminal'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DatabricksWorkspace 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DatabricksWorkspace'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DatabricksWorkspace'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DataTransferOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DataTransferOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DataTransferOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DCRLogErrors 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DCRLogErrors'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DCRLogErrors'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DCRLogTroubleshooting 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DCRLogTroubleshooting'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DCRLogTroubleshooting'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DevCenterDiagnosticLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DevCenterDiagnosticLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DevCenterDiagnosticLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DevCenterResourceOperationLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DevCenterResourceOperationLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DevCenterResourceOperationLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DSMAzureBlobStorageLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DSMAzureBlobStorageLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DSMAzureBlobStorageLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DSMDataClassificationLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DSMDataClassificationLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DSMDataClassificationLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_DSMDataLabelingLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'DSMDataLabelingLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'DSMDataLabelingLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_EnrichedMicrosoft365AuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'EnrichedMicrosoft365AuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'EnrichedMicrosoft365AuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ETWEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ETWEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ETWEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_Event 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'Event'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'Event'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ExchangeAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ExchangeAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ExchangeAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ExchangeOnlineAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ExchangeOnlineAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ExchangeOnlineAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_FailedIngestion 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'FailedIngestion'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'FailedIngestion'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_FunctionAppLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'FunctionAppLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'FunctionAppLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightAmbariClusterAlerts 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightAmbariClusterAlerts'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightAmbariClusterAlerts'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightAmbariSystemMetrics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightAmbariSystemMetrics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightAmbariSystemMetrics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightGatewayAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightGatewayAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightGatewayAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightHadoopAndYarnLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightHadoopAndYarnLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightHadoopAndYarnLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightHadoopAndYarnMetrics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightHadoopAndYarnMetrics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightHadoopAndYarnMetrics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightHBaseLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightHBaseLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightHBaseLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightHBaseMetrics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightHBaseMetrics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightHBaseMetrics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightHiveAndLLAPLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightHiveAndLLAPLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightHiveAndLLAPLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightHiveAndLLAPMetrics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightHiveAndLLAPMetrics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightHiveAndLLAPMetrics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightHiveQueryAppStats 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightHiveQueryAppStats'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightHiveQueryAppStats'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightHiveTezAppStats 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightHiveTezAppStats'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightHiveTezAppStats'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightJupyterNotebookEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightJupyterNotebookEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightJupyterNotebookEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightKafkaLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightKafkaLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightKafkaLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightKafkaMetrics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightKafkaMetrics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightKafkaMetrics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightKafkaServerLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightKafkaServerLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightKafkaServerLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightOozieLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightOozieLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightOozieLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightRangerAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightRangerAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightRangerAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSecurityLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSecurityLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSecurityLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSparkApplicationEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSparkApplicationEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSparkApplicationEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSparkBlockManagerEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSparkBlockManagerEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSparkBlockManagerEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSparkEnvironmentEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSparkEnvironmentEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSparkEnvironmentEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSparkExecutorEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSparkExecutorEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSparkExecutorEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSparkExtraEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSparkExtraEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSparkExtraEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSparkJobEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSparkJobEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSparkJobEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSparkLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSparkLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSparkLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSparkSQLExecutionEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSparkSQLExecutionEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSparkSQLExecutionEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSparkStageEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSparkStageEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSparkStageEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSparkStageTaskAccumulables 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSparkStageTaskAccumulables'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSparkStageTaskAccumulables'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightSparkTaskEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightSparkTaskEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightSparkTaskEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightStormLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightStormLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightStormLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightStormMetrics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightStormMetrics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightStormMetrics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_HDInsightStormTopologyMetrics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'HDInsightStormTopologyMetrics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'HDInsightStormTopologyMetrics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_Heartbeat 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'Heartbeat'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'Heartbeat'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_InsightsMetrics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'InsightsMetrics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'InsightsMetrics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_IntuneAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'IntuneAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'IntuneAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_IntuneDeviceComplianceOrg 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'IntuneDeviceComplianceOrg'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'IntuneDeviceComplianceOrg'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_IntuneDevices 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'IntuneDevices'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'IntuneDevices'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_IntuneOperationalLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'IntuneOperationalLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'IntuneOperationalLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_KubeEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'KubeEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'KubeEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_KubeHealth 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'KubeHealth'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'KubeHealth'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_KubeMonAgentEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'KubeMonAgentEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'KubeMonAgentEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_KubeNodeInventory 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'KubeNodeInventory'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'KubeNodeInventory'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_KubePodInventory 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'KubePodInventory'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'KubePodInventory'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_KubePVInventory 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'KubePVInventory'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'KubePVInventory'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_KubeServices 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'KubeServices'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'KubeServices'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_LAQueryLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LAQueryLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'LAQueryLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_LogicAppWorkflowRuntime 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'LogicAppWorkflowRuntime'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'LogicAppWorkflowRuntime'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_MCCEventLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'MCCEventLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'MCCEventLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_MCVPAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'MCVPAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'MCVPAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_MCVPOperationLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'MCVPOperationLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'MCVPOperationLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_MicrosoftAzureBastionAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'MicrosoftAzureBastionAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'MicrosoftAzureBastionAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_MicrosoftDataShareReceivedSnapshotLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'MicrosoftDataShareReceivedSnapshotLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'MicrosoftDataShareReceivedSnapshotLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_MicrosoftDataShareSentSnapshotLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'MicrosoftDataShareSentSnapshotLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'MicrosoftDataShareSentSnapshotLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_MicrosoftDataShareShareLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'MicrosoftDataShareShareLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'MicrosoftDataShareShareLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_MicrosoftGraphActivityLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'MicrosoftGraphActivityLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'MicrosoftGraphActivityLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_MicrosoftHealthcareApisAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'MicrosoftHealthcareApisAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'MicrosoftHealthcareApisAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NCBMSecurityLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NCBMSecurityLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NCBMSecurityLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NCBMSystemLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NCBMSystemLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NCBMSystemLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NCCKubernetesLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NCCKubernetesLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NCCKubernetesLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NCCVMOrchestrationLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NCCVMOrchestrationLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NCCVMOrchestrationLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NCSStorageAlerts 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NCSStorageAlerts'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NCSStorageAlerts'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NCSStorageLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NCSStorageLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NCSStorageLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NetworkAccessTraffic 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NetworkAccessTraffic'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NetworkAccessTraffic'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NSPAccessLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NSPAccessLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NSPAccessLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NTAIpDetails 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NTAIpDetails'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NTAIpDetails'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NTANetAnalytics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NTANetAnalytics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NTANetAnalytics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NTATopologyDetails 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NTATopologyDetails'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NTATopologyDetails'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NWConnectionMonitorDNSResult 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NWConnectionMonitorDNSResult'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NWConnectionMonitorDNSResult'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NWConnectionMonitorPathResult 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NWConnectionMonitorPathResult'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NWConnectionMonitorPathResult'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_NWConnectionMonitorTestResult 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'NWConnectionMonitorTestResult'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'NWConnectionMonitorTestResult'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_OEPAirFlowTask 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'OEPAirFlowTask'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'OEPAirFlowTask'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_OEPAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'OEPAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'OEPAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_OEPDataplaneLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'OEPDataplaneLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'OEPDataplaneLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_OEPElasticOperator 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'OEPElasticOperator'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'OEPElasticOperator'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_OEPElasticsearch 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'OEPElasticsearch'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'OEPElasticsearch'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_OLPSupplyChainEntityOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'OLPSupplyChainEntityOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'OLPSupplyChainEntityOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_OLPSupplyChainEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'OLPSupplyChainEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'OLPSupplyChainEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_Operation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'Operation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'Operation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_Perf 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'Perf'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'Perf'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_PFTitleAuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'PFTitleAuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'PFTitleAuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_PowerBIAuditTenant 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'PowerBIAuditTenant'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'PowerBIAuditTenant'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_PowerBIDatasetsTenant 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'PowerBIDatasetsTenant'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'PowerBIDatasetsTenant'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_PowerBIDatasetsWorkspace 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'PowerBIDatasetsWorkspace'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'PowerBIDatasetsWorkspace'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_PowerBIReportUsageTenant 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'PowerBIReportUsageTenant'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'PowerBIReportUsageTenant'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_PowerBIReportUsageWorkspace 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'PowerBIReportUsageWorkspace'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'PowerBIReportUsageWorkspace'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_PurviewDataSensitivityLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'PurviewDataSensitivityLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'PurviewDataSensitivityLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_PurviewScanStatusLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'PurviewScanStatusLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'PurviewScanStatusLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_PurviewSecurityLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'PurviewSecurityLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'PurviewSecurityLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_REDConnectionEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'REDConnectionEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'REDConnectionEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ResourceManagementPublicAccessLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ResourceManagementPublicAccessLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ResourceManagementPublicAccessLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SCCMAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SCCMAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SCCMAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SCOMAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SCOMAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SCOMAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ServiceFabricOperationalEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ServiceFabricOperationalEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ServiceFabricOperationalEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ServiceFabricReliableActorEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ServiceFabricReliableActorEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ServiceFabricReliableActorEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_ServiceFabricReliableServiceEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'ServiceFabricReliableServiceEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'ServiceFabricReliableServiceEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SfBAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SfBAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SfBAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SfBOnlineAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SfBOnlineAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SfBOnlineAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SharePointOnlineAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SharePointOnlineAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SharePointOnlineAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SignalRServiceDiagnosticLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SignalRServiceDiagnosticLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SignalRServiceDiagnosticLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SigninLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SigninLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SigninLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SPAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SPAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SPAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SQLAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SQLAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SQLAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SQLSecurityAuditEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SQLSecurityAuditEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SQLSecurityAuditEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageAntimalwareScanResults 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageAntimalwareScanResults'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageAntimalwareScanResults'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageBlobLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageBlobLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageBlobLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageCacheOperationEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageCacheOperationEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageCacheOperationEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageCacheUpgradeEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageCacheUpgradeEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageCacheUpgradeEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageCacheWarningEvents 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageCacheWarningEvents'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageCacheWarningEvents'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageFileLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageFileLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageFileLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageMalwareScanningResults 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageMalwareScanningResults'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageMalwareScanningResults'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageMoverCopyLogsFailed 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageMoverCopyLogsFailed'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageMoverCopyLogsFailed'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageMoverCopyLogsTransferred 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageMoverCopyLogsTransferred'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageMoverCopyLogsTransferred'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageMoverJobRunLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageMoverJobRunLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageMoverJobRunLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageQueueLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageQueueLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageQueueLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_StorageTableLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'StorageTableLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'StorageTableLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SucceededIngestion 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SucceededIngestion'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SucceededIngestion'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseBigDataPoolApplicationsEnded 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseBigDataPoolApplicationsEnded'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseBigDataPoolApplicationsEnded'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseBuiltinSqlPoolRequestsEnded 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseBuiltinSqlPoolRequestsEnded'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseBuiltinSqlPoolRequestsEnded'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseDXCommand 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseDXCommand'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseDXCommand'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseDXFailedIngestion 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseDXFailedIngestion'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseDXFailedIngestion'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseDXIngestionBatching 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseDXIngestionBatching'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseDXIngestionBatching'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseDXQuery 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseDXQuery'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseDXQuery'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseDXSucceededIngestion 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseDXSucceededIngestion'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseDXSucceededIngestion'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseDXTableDetails 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseDXTableDetails'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseDXTableDetails'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseDXTableUsageStatistics 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseDXTableUsageStatistics'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseDXTableUsageStatistics'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseGatewayApiRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseGatewayApiRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseGatewayApiRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseIntegrationActivityRuns 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseIntegrationActivityRuns'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseIntegrationActivityRuns'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseIntegrationPipelineRuns 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseIntegrationPipelineRuns'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseIntegrationPipelineRuns'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseIntegrationTriggerRuns 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseIntegrationTriggerRuns'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseIntegrationTriggerRuns'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseLinkEvent 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseLinkEvent'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseLinkEvent'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseRbacOperations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseRbacOperations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseRbacOperations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseScopePoolScopeJobsEnded 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseScopePoolScopeJobsEnded'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseScopePoolScopeJobsEnded'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseScopePoolScopeJobsStateChange 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseScopePoolScopeJobsStateChange'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseScopePoolScopeJobsStateChange'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseSqlPoolDmsWorkers 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseSqlPoolDmsWorkers'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseSqlPoolDmsWorkers'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseSqlPoolExecRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseSqlPoolExecRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseSqlPoolExecRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseSqlPoolRequestSteps 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseSqlPoolRequestSteps'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseSqlPoolRequestSteps'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseSqlPoolSqlRequests 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseSqlPoolSqlRequests'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseSqlPoolSqlRequests'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_SynapseSqlPoolWaits 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'SynapseSqlPoolWaits'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'SynapseSqlPoolWaits'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_Syslog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'Syslog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'Syslog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_TSIIngress 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'TSIIngress'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'TSIIngress'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_UCClient 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'UCClient'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'UCClient'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_UCClientReadinessStatus 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'UCClientReadinessStatus'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'UCClientReadinessStatus'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_UCClientUpdateStatus 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'UCClientUpdateStatus'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'UCClientUpdateStatus'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_UCDeviceAlert 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'UCDeviceAlert'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'UCDeviceAlert'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_UCDOAggregatedStatus 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'UCDOAggregatedStatus'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'UCDOAggregatedStatus'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_UCDOStatus 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'UCDOStatus'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'UCDOStatus'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_UCServiceUpdateStatus 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'UCServiceUpdateStatus'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'UCServiceUpdateStatus'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_UCUpdateAlert 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'UCUpdateAlert'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'UCUpdateAlert'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_Usage 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'Usage'
  properties: {
    plan: 'Analytics'
    retentionInDays: 90
    schema: {
      name: 'Usage'
    }
    totalRetentionInDays: 90
  }
}

resource workspaces_workspaceyourbrand8c41_name_VIAudit 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'VIAudit'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'VIAudit'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_VIIndexing 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'VIIndexing'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'VIIndexing'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_VMBoundPort 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'VMBoundPort'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'VMBoundPort'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_VMComputer 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'VMComputer'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'VMComputer'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_VMConnection 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'VMConnection'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'VMConnection'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_VMProcess 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'VMProcess'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'VMProcess'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_W3CIISLog 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'W3CIISLog'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'W3CIISLog'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WebPubSubConnectivity 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WebPubSubConnectivity'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WebPubSubConnectivity'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WebPubSubHttpRequest 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WebPubSubHttpRequest'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WebPubSubHttpRequest'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WebPubSubMessaging 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WebPubSubMessaging'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WebPubSubMessaging'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_Windows365AuditLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'Windows365AuditLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'Windows365AuditLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WindowsClientAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WindowsClientAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WindowsClientAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WindowsServerAssessmentRecommendation 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WindowsServerAssessmentRecommendation'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WindowsServerAssessmentRecommendation'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WorkloadDiagnosticLogs 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WorkloadDiagnosticLogs'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WorkloadDiagnosticLogs'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WVDAgentHealthStatus 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WVDAgentHealthStatus'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WVDAgentHealthStatus'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WVDAutoscaleEvaluationPooled 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WVDAutoscaleEvaluationPooled'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WVDAutoscaleEvaluationPooled'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WVDCheckpoints 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WVDCheckpoints'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WVDCheckpoints'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WVDConnectionGraphicsDataPreview 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WVDConnectionGraphicsDataPreview'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WVDConnectionGraphicsDataPreview'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WVDConnectionNetworkData 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WVDConnectionNetworkData'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WVDConnectionNetworkData'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WVDConnections 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WVDConnections'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WVDConnections'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WVDErrors 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WVDErrors'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WVDErrors'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WVDFeeds 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WVDFeeds'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WVDFeeds'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WVDHostRegistrations 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WVDHostRegistrations'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WVDHostRegistrations'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WVDManagement 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WVDManagement'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WVDManagement'
    }
    totalRetentionInDays: 30
  }
}

resource workspaces_workspaceyourbrand8c41_name_WVDSessionHostManagement 'Microsoft.OperationalInsights/workspaces/tables@2021-12-01-preview' = {
  parent: workspaces_workspaceyourbrand8c41_name_resource
  name: 'WVDSessionHostManagement'
  properties: {
    plan: 'Analytics'
    retentionInDays: 30
    schema: {
      name: 'WVDSessionHostManagement'
    }
    totalRetentionInDays: 30
  }
}

resource namespaces_yourbrand_servicebus_name_RootManageSharedAccessKey 'Microsoft.ServiceBus/namespaces/authorizationrules@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'RootManageSharedAccessKey'
  properties: {
    rights: [
      'Listen'
      'Manage'
      'Send'
    ]
  }
}

resource namespaces_yourbrand_servicebus_name_default 'Microsoft.ServiceBus/namespaces/networkrulesets@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'default'
  properties: {
    defaultAction: 'Allow'
    ipRules: []
    publicNetworkAccess: 'Enabled'
    trustedServiceAccessEnabled: false
    virtualNetworkRules: []
  }
}

resource namespaces_yourbrand_servicebus_name_add_cart_item 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'add-cart-item'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    lockDuration: 'PT5M'
    maxDeliveryCount: 5
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_add_cart_item_error 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'add-cart-item_error'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    lockDuration: 'PT5M'
    maxDeliveryCount: 5
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_get_cart_by_id 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'get-cart-by-id'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    lockDuration: 'PT5M'
    maxDeliveryCount: 5
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_get_cart_by_id_error 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'get-cart-by-id_error'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    lockDuration: 'PT5M'
    maxDeliveryCount: 5
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_get_carts 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'get-carts'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    lockDuration: 'PT5M'
    maxDeliveryCount: 5
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_product_details_updated 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'product-details-updated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    lockDuration: 'PT5M'
    maxDeliveryCount: 5
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_product_handle_updated 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'product-handle-updated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    lockDuration: 'PT5M'
    maxDeliveryCount: 5
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_product_image_updated 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'product-image-updated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    lockDuration: 'PT5M'
    maxDeliveryCount: 5
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_product_price_updated 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'product-price-updated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    lockDuration: 'PT5M'
    maxDeliveryCount: 5
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_remove_cart_item_quantity 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'remove-cart-item-quantity'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    lockDuration: 'PT5M'
    maxDeliveryCount: 5
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_update_cart_item_quantity 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'update-cart-item-quantity'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    lockDuration: 'PT5M'
    maxDeliveryCount: 5
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_addcartitem 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'carts.contracts~addcartitem'
  properties: {
    autoDeleteOnIdle: 'P427D'
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    status: 'Active'
    supportOrdering: false
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_getcartbyid 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'carts.contracts~getcartbyid'
  properties: {
    autoDeleteOnIdle: 'P427D'
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    status: 'Active'
    supportOrdering: false
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_getcarts 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'carts.contracts~getcarts'
  properties: {
    autoDeleteOnIdle: 'P427D'
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    status: 'Active'
    supportOrdering: false
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_removecartitem 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'carts.contracts~removecartitem'
  properties: {
    autoDeleteOnIdle: 'P427D'
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    status: 'Active'
    supportOrdering: false
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_updatecartitemquantity 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'carts.contracts~updatecartitemquantity'
  properties: {
    autoDeleteOnIdle: 'P427D'
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    status: 'Active'
    supportOrdering: false
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_productdetailsupdated 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'catalog.contracts~productdetailsupdated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    status: 'Active'
    supportOrdering: false
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_producthandleupdated 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'catalog.contracts~producthandleupdated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    status: 'Active'
    supportOrdering: false
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_productimageupdated 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'catalog.contracts~productimageupdated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    status: 'Active'
    supportOrdering: false
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_productpriceupdated 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_resource
  location: location
  name: 'catalog.contracts~productpriceupdated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    defaultMessageTimeToLive: 'P366D'
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    enableBatchedOperations: true
    enableExpress: false
    enablePartitioning: false
    maxMessageSizeInKilobytes: 256
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    status: 'Active'
    supportOrdering: false
  }
}

resource servers_yourbrand_sqlserver_name_ActiveDirectory 'Microsoft.Sql/servers/administrators@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'ActiveDirectory'
  properties: {
    administratorType: 'ActiveDirectory'
    login: 'robert.sundstrom_outlook.com#EXT#@robertsundstromoutlook.onmicrosoft.com'
    sid: 'a37f7582-181d-46d5-8403-f45c75dd39b2'
    tenantId: '99305724-84f9-474d-aa7b-759e7b4d38d2'
  }
}

resource servers_yourbrand_sqlserver_name_Default 'Microsoft.Sql/servers/advancedThreatProtectionSettings@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'Default'
  properties: {
    state: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_CreateIndex 'Microsoft.Sql/servers/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'CreateIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_DbParameterization 'Microsoft.Sql/servers/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'DbParameterization'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_DefragmentIndex 'Microsoft.Sql/servers/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'DefragmentIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_DropIndex 'Microsoft.Sql/servers/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'DropIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_ForceLastGoodPlan 'Microsoft.Sql/servers/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'ForceLastGoodPlan'
  properties: {
    autoExecuteValue: 'Enabled'
  }
}

resource Microsoft_Sql_servers_auditingPolicies_servers_yourbrand_sqlserver_name_Default 'Microsoft.Sql/servers/auditingPolicies@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_resource
  location: location
  name: 'Default'
  properties: {
    auditingState: 'Disabled'
  }
}

resource Microsoft_Sql_servers_auditingSettings_servers_yourbrand_sqlserver_name_Default 'Microsoft.Sql/servers/auditingSettings@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'default'
  properties: {
    auditActionsAndGroups: []
    isAzureMonitorTargetEnabled: false
    isManagedIdentityInUse: false
    isStorageSecondaryKeyInUse: false
    retentionDays: 0
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
}

resource Microsoft_Sql_servers_azureADOnlyAuthentications_servers_yourbrand_sqlserver_name_Default 'Microsoft.Sql/servers/azureADOnlyAuthentications@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'Default'
  properties: {
    azureADOnlyAuthentication: false
  }
}

resource Microsoft_Sql_servers_connectionPolicies_servers_yourbrand_sqlserver_name_default 'Microsoft.Sql/servers/connectionPolicies@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  location: location
  name: 'default'
  properties: {
    connectionType: 'Default'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_carts_db 'Microsoft.Sql/servers/databases@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
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

resource servers_yourbrand_sqlserver_name_yourbrand_catalog_db 'Microsoft.Sql/servers/databases@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
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

resource servers_yourbrand_sqlserver_name_master_Default 'Microsoft.Sql/servers/databases/advancedThreatProtectionSettings@2023-02-01-preview' = {
  name: '${servers_yourbrand_sqlserver_name}/master/Default'
  properties: {
    state: 'Disabled'
  }
  dependsOn: [
    servers_yourbrand_sqlserver_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_auditingPolicies_servers_yourbrand_sqlserver_name_master_Default 'Microsoft.Sql/servers/databases/auditingPolicies@2014-04-01' = {
  location: location
  name: '${servers_yourbrand_sqlserver_name}/master/Default'
  properties: {
    auditingState: 'Disabled'
  }
  dependsOn: [
    servers_yourbrand_sqlserver_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_auditingSettings_servers_yourbrand_sqlserver_name_master_Default 'Microsoft.Sql/servers/databases/auditingSettings@2023-02-01-preview' = {
  name: '${servers_yourbrand_sqlserver_name}/master/Default'
  properties: {
    isAzureMonitorTargetEnabled: false
    retentionDays: 0
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
  dependsOn: [
    servers_yourbrand_sqlserver_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_extendedAuditingSettings_servers_yourbrand_sqlserver_name_master_Default 'Microsoft.Sql/servers/databases/extendedAuditingSettings@2023-02-01-preview' = {
  name: '${servers_yourbrand_sqlserver_name}/master/Default'
  properties: {
    isAzureMonitorTargetEnabled: false
    retentionDays: 0
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
  dependsOn: [
    servers_yourbrand_sqlserver_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_geoBackupPolicies_servers_yourbrand_sqlserver_name_master_Default 'Microsoft.Sql/servers/databases/geoBackupPolicies@2023-02-01-preview' = {
  name: '${servers_yourbrand_sqlserver_name}/master/Default'
  properties: {
    state: 'Enabled'
  }
  dependsOn: [
    servers_yourbrand_sqlserver_name_resource
  ]
}

resource servers_yourbrand_sqlserver_name_master_Current 'Microsoft.Sql/servers/databases/ledgerDigestUploads@2023-02-01-preview' = {
  name: '${servers_yourbrand_sqlserver_name}/master/Current'
  properties: {}
  dependsOn: [
    servers_yourbrand_sqlserver_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_securityAlertPolicies_servers_yourbrand_sqlserver_name_master_Default 'Microsoft.Sql/servers/databases/securityAlertPolicies@2023-02-01-preview' = {
  name: '${servers_yourbrand_sqlserver_name}/master/Default'
  properties: {
    disabledAlerts: [
      ''
    ]
    emailAccountAdmins: false
    emailAddresses: [
      ''
    ]
    retentionDays: 0
    state: 'Disabled'
  }
  dependsOn: [
    servers_yourbrand_sqlserver_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_transparentDataEncryption_servers_yourbrand_sqlserver_name_master_Current 'Microsoft.Sql/servers/databases/transparentDataEncryption@2023-02-01-preview' = {
  name: '${servers_yourbrand_sqlserver_name}/master/Current'
  properties: {
    state: 'Disabled'
  }
  dependsOn: [
    servers_yourbrand_sqlserver_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_vulnerabilityAssessments_servers_yourbrand_sqlserver_name_master_Default 'Microsoft.Sql/servers/databases/vulnerabilityAssessments@2023-02-01-preview' = {
  name: '${servers_yourbrand_sqlserver_name}/master/Default'
  properties: {
    recurringScans: {
      emailSubscriptionAdmins: true
      isEnabled: false
    }
  }
  dependsOn: [
    servers_yourbrand_sqlserver_name_resource
  ]
}

resource Microsoft_Sql_servers_devOpsAuditingSettings_servers_yourbrand_sqlserver_name_Default 'Microsoft.Sql/servers/devOpsAuditingSettings@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'Default'
  properties: {
    isAzureMonitorTargetEnabled: false
    isManagedIdentityInUse: false
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
}

resource servers_yourbrand_sqlserver_name_current 'Microsoft.Sql/servers/encryptionProtector@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  kind: 'servicemanaged'
  name: 'current'
  properties: {
    autoRotationEnabled: false
    serverKeyName: 'ServiceManaged'
    serverKeyType: 'ServiceManaged'
  }
}

resource Microsoft_Sql_servers_extendedAuditingSettings_servers_yourbrand_sqlserver_name_Default 'Microsoft.Sql/servers/extendedAuditingSettings@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'default'
  properties: {
    auditActionsAndGroups: []
    isAzureMonitorTargetEnabled: false
    isManagedIdentityInUse: false
    isStorageSecondaryKeyInUse: false
    retentionDays: 0
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
}

resource servers_yourbrand_sqlserver_name_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallRules@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'AllowAllWindowsAzureIps'
}

resource servers_yourbrand_sqlserver_name_ClientIPAddress_2023_10_01_17_21_33 'Microsoft.Sql/servers/firewallRules@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'ClientIPAddress_2023-10-01_17-21-33'
}

resource servers_yourbrand_sqlserver_name_ClientIPAddress_2023_9_30_19_48_38 'Microsoft.Sql/servers/firewallRules@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'ClientIPAddress_2023-9-30_19-48-38'
}

resource servers_yourbrand_sqlserver_name_ServiceManaged 'Microsoft.Sql/servers/keys@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  kind: 'servicemanaged'
  name: 'ServiceManaged'
  properties: {
    serverKeyType: 'ServiceManaged'
  }
}

resource Microsoft_Sql_servers_securityAlertPolicies_servers_yourbrand_sqlserver_name_Default 'Microsoft.Sql/servers/securityAlertPolicies@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'Default'
  properties: {
    disabledAlerts: [
      ''
    ]
    emailAccountAdmins: false
    emailAddresses: [
      ''
    ]
    retentionDays: 0
    state: 'Disabled'
  }
}

resource Microsoft_Sql_servers_sqlVulnerabilityAssessments_servers_yourbrand_sqlserver_name_Default 'Microsoft.Sql/servers/sqlVulnerabilityAssessments@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'Default'
  properties: {
    state: 'Disabled'
  }
}

resource Microsoft_Sql_servers_vulnerabilityAssessments_servers_yourbrand_sqlserver_name_Default 'Microsoft.Sql/servers/vulnerabilityAssessments@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_resource
  name: 'Default'
  properties: {
    recurringScans: {
      emailSubscriptionAdmins: true
      isEnabled: false
    }
    storageContainerPath: vulnerabilityAssessments_Default_storageContainerPath
  }
}

resource storageAccounts_yourbrandstorage_name_default 'Microsoft.Storage/storageAccounts/blobServices@2023-01-01' = {
  parent: storageAccounts_yourbrandstorage_name_resource
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
  parent: storageAccounts_yourbrandstorage_name_resource
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
  parent: storageAccounts_yourbrandstorage_name_resource
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}

resource Microsoft_Storage_storageAccounts_tableServices_storageAccounts_yourbrandstorage_name_default 'Microsoft.Storage/storageAccounts/tableServices@2023-01-01' = {
  parent: storageAccounts_yourbrandstorage_name_resource
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_addcartitem_add_cart_item 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_carts_contracts_addcartitem
  location: location
  name: 'add-cart-item'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnFilterEvaluationExceptions: true
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P366D'
    enableBatchedOperations: true
    forwardTo: 'add-cart-item'
    isClientAffine: false
    lockDuration: 'PT1M'
    maxDeliveryCount: 10
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_getcartbyid_get_cart_by_id 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_carts_contracts_getcartbyid
  location: location
  name: 'get-cart-by-id'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnFilterEvaluationExceptions: true
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P366D'
    enableBatchedOperations: true
    forwardTo: 'get-cart-by-id'
    isClientAffine: false
    lockDuration: 'PT1M'
    maxDeliveryCount: 10
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_getcarts_get_carts 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_carts_contracts_getcarts
  location: location
  name: 'get-carts'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnFilterEvaluationExceptions: true
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P366D'
    enableBatchedOperations: true
    forwardTo: 'get-carts'
    isClientAffine: false
    lockDuration: 'PT1M'
    maxDeliveryCount: 10
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_productdetailsupdated_product_details_updated 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_catalog_contracts_productdetailsupdated
  location: location
  name: 'product-details-updated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnFilterEvaluationExceptions: true
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P366D'
    enableBatchedOperations: true
    forwardTo: 'product-details-updated'
    isClientAffine: false
    lockDuration: 'PT1M'
    maxDeliveryCount: 10
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_producthandleupdated_product_handle_updated 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_catalog_contracts_producthandleupdated
  location: location
  name: 'product-handle-updated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnFilterEvaluationExceptions: true
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P366D'
    enableBatchedOperations: true
    forwardTo: 'product-handle-updated'
    isClientAffine: false
    lockDuration: 'PT1M'
    maxDeliveryCount: 10
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_productimageupdated_product_image_updated 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_catalog_contracts_productimageupdated
  location: location
  name: 'product-image-updated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnFilterEvaluationExceptions: true
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P366D'
    enableBatchedOperations: true
    forwardTo: 'product-image-updated'
    isClientAffine: false
    lockDuration: 'PT1M'
    maxDeliveryCount: 10
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_productpriceupdated_product_price_updated 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_catalog_contracts_productpriceupdated
  location: location
  name: 'product-price-updated'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnFilterEvaluationExceptions: true
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P366D'
    enableBatchedOperations: true
    forwardTo: 'product-price-updated'
    isClientAffine: false
    lockDuration: 'PT1M'
    maxDeliveryCount: 10
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_removecartitem_remove_cart_item_quantity 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_carts_contracts_removecartitem
  location: location
  name: 'remove-cart-item-quantity'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnFilterEvaluationExceptions: true
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P366D'
    enableBatchedOperations: true
    forwardTo: 'remove-cart-item-quantity'
    isClientAffine: false
    lockDuration: 'PT1M'
    maxDeliveryCount: 10
    requiresSession: false
    status: 'Active'
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_updatecartitemquantity_update_cart_item_quantity 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_carts_contracts_updatecartitemquantity
  location: location
  name: 'update-cart-item-quantity'
  properties: {
    autoDeleteOnIdle: 'P427D'
    deadLetteringOnFilterEvaluationExceptions: true
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P366D'
    enableBatchedOperations: true
    forwardTo: 'update-cart-item-quantity'
    isClientAffine: false
    lockDuration: 'PT1M'
    maxDeliveryCount: 10
    requiresSession: false
    status: 'Active'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_carts_db_Default 'Microsoft.Sql/servers/databases/advancedThreatProtectionSettings@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'Default'
  properties: {
    state: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_catalog_db_Default 'Microsoft.Sql/servers/databases/advancedThreatProtectionSettings@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'Default'
  properties: {
    state: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_carts_db_CreateIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'CreateIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_catalog_db_CreateIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'CreateIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_carts_db_DbParameterization 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'DbParameterization'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_catalog_db_DbParameterization 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'DbParameterization'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_carts_db_DefragmentIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'DefragmentIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_catalog_db_DefragmentIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'DefragmentIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_carts_db_DropIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'DropIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_catalog_db_DropIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'DropIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_carts_db_ForceLastGoodPlan 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'ForceLastGoodPlan'
  properties: {
    autoExecuteValue: 'Enabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_catalog_db_ForceLastGoodPlan 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'ForceLastGoodPlan'
  properties: {
    autoExecuteValue: 'Enabled'
  }
}

resource Microsoft_Sql_servers_databases_auditingPolicies_servers_yourbrand_sqlserver_name_yourbrand_carts_db_Default 'Microsoft.Sql/servers/databases/auditingPolicies@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  location: location
  name: 'Default'
  properties: {
    auditingState: 'Disabled'
  }
}

resource Microsoft_Sql_servers_databases_auditingPolicies_servers_yourbrand_sqlserver_name_yourbrand_catalog_db_Default 'Microsoft.Sql/servers/databases/auditingPolicies@2014-04-01' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  location: location
  name: 'Default'
  properties: {
    auditingState: 'Disabled'
  }
}

resource Microsoft_Sql_servers_databases_auditingSettings_servers_yourbrand_sqlserver_name_yourbrand_carts_db_Default 'Microsoft.Sql/servers/databases/auditingSettings@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'default'
  properties: {
    isAzureMonitorTargetEnabled: false
    retentionDays: 0
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
}

resource Microsoft_Sql_servers_databases_auditingSettings_servers_yourbrand_sqlserver_name_yourbrand_catalog_db_Default 'Microsoft.Sql/servers/databases/auditingSettings@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'default'
  properties: {
    isAzureMonitorTargetEnabled: false
    retentionDays: 0
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
}

resource Microsoft_Sql_servers_databases_backupLongTermRetentionPolicies_servers_yourbrand_sqlserver_name_yourbrand_carts_db_default 'Microsoft.Sql/servers/databases/backupLongTermRetentionPolicies@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'default'
  properties: {
    makeBackupsImmutable: false
    monthlyRetention: 'PT0S'
    weekOfYear: 0
    weeklyRetention: 'PT0S'
    yearlyRetention: 'PT0S'
  }
}

resource Microsoft_Sql_servers_databases_backupLongTermRetentionPolicies_servers_yourbrand_sqlserver_name_yourbrand_catalog_db_default 'Microsoft.Sql/servers/databases/backupLongTermRetentionPolicies@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'default'
  properties: {
    makeBackupsImmutable: false
    monthlyRetention: 'PT0S'
    weekOfYear: 0
    weeklyRetention: 'PT0S'
    yearlyRetention: 'PT0S'
  }
}

resource Microsoft_Sql_servers_databases_backupShortTermRetentionPolicies_servers_yourbrand_sqlserver_name_yourbrand_carts_db_default 'Microsoft.Sql/servers/databases/backupShortTermRetentionPolicies@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'default'
  properties: {
    diffBackupIntervalInHours: 24
    retentionDays: 7
  }
}

resource Microsoft_Sql_servers_databases_backupShortTermRetentionPolicies_servers_yourbrand_sqlserver_name_yourbrand_catalog_db_default 'Microsoft.Sql/servers/databases/backupShortTermRetentionPolicies@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'default'
  properties: {
    diffBackupIntervalInHours: 24
    retentionDays: 7
  }
}

resource Microsoft_Sql_servers_databases_extendedAuditingSettings_servers_yourbrand_sqlserver_name_yourbrand_carts_db_Default 'Microsoft.Sql/servers/databases/extendedAuditingSettings@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'default'
  properties: {
    isAzureMonitorTargetEnabled: false
    retentionDays: 0
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
}

resource Microsoft_Sql_servers_databases_extendedAuditingSettings_servers_yourbrand_sqlserver_name_yourbrand_catalog_db_Default 'Microsoft.Sql/servers/databases/extendedAuditingSettings@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'default'
  properties: {
    isAzureMonitorTargetEnabled: false
    retentionDays: 0
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
}

resource Microsoft_Sql_servers_databases_geoBackupPolicies_servers_yourbrand_sqlserver_name_yourbrand_carts_db_Default 'Microsoft.Sql/servers/databases/geoBackupPolicies@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'Default'
  properties: {
    state: 'Disabled'
  }
}

resource Microsoft_Sql_servers_databases_geoBackupPolicies_servers_yourbrand_sqlserver_name_yourbrand_catalog_db_Default 'Microsoft.Sql/servers/databases/geoBackupPolicies@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'Default'
  properties: {
    state: 'Disabled'
  }
}

resource servers_yourbrand_sqlserver_name_yourbrand_carts_db_Current 'Microsoft.Sql/servers/databases/ledgerDigestUploads@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'Current'
  properties: {}
}

resource servers_yourbrand_sqlserver_name_yourbrand_catalog_db_Current 'Microsoft.Sql/servers/databases/ledgerDigestUploads@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'Current'
  properties: {}
}

resource Microsoft_Sql_servers_databases_securityAlertPolicies_servers_yourbrand_sqlserver_name_yourbrand_carts_db_Default 'Microsoft.Sql/servers/databases/securityAlertPolicies@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'Default'
  properties: {
    disabledAlerts: [
      ''
    ]
    emailAccountAdmins: false
    emailAddresses: [
      ''
    ]
    retentionDays: 0
    state: 'Disabled'
  }
}

resource Microsoft_Sql_servers_databases_securityAlertPolicies_servers_yourbrand_sqlserver_name_yourbrand_catalog_db_Default 'Microsoft.Sql/servers/databases/securityAlertPolicies@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'Default'
  properties: {
    disabledAlerts: [
      ''
    ]
    emailAccountAdmins: false
    emailAddresses: [
      ''
    ]
    retentionDays: 0
    state: 'Disabled'
  }
}

resource Microsoft_Sql_servers_databases_transparentDataEncryption_servers_yourbrand_sqlserver_name_yourbrand_carts_db_Current 'Microsoft.Sql/servers/databases/transparentDataEncryption@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'Current'
  properties: {
    state: 'Enabled'
  }
}

resource Microsoft_Sql_servers_databases_transparentDataEncryption_servers_yourbrand_sqlserver_name_yourbrand_catalog_db_Current 'Microsoft.Sql/servers/databases/transparentDataEncryption@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'Current'
  properties: {
    state: 'Enabled'
  }
}

resource Microsoft_Sql_servers_databases_vulnerabilityAssessments_servers_yourbrand_sqlserver_name_yourbrand_carts_db_Default 'Microsoft.Sql/servers/databases/vulnerabilityAssessments@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_carts_db
  name: 'Default'
  properties: {
    recurringScans: {
      emailSubscriptionAdmins: true
      isEnabled: false
    }
  }
}

resource Microsoft_Sql_servers_databases_vulnerabilityAssessments_servers_yourbrand_sqlserver_name_yourbrand_catalog_db_Default 'Microsoft.Sql/servers/databases/vulnerabilityAssessments@2023-02-01-preview' = {
  parent: servers_yourbrand_sqlserver_name_yourbrand_catalog_db
  name: 'Default'
  properties: {
    recurringScans: {
      emailSubscriptionAdmins: true
      isEnabled: false
    }
  }
}

resource storageAccounts_yourbrandstorage_name_default_images 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-01-01' = {
  parent: storageAccounts_yourbrandstorage_name_default
  name: 'images'
  properties: {
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    immutableStorageWithVersioning: {
      enabled: false
    }
    publicAccess: 'Blob'
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_addcartitem_add_cart_item_Default 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_carts_contracts_addcartitem_add_cart_item
  location: location
  name: '$Default'
  properties: {
    action: {}
    filterType: 'SqlFilter'
    sqlFilter: {
      compatibilityLevel: 20
      sqlExpression: '1=1'
    }
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_getcartbyid_get_cart_by_id_Default 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_carts_contracts_getcartbyid_get_cart_by_id
  location: location
  name: '$Default'
  properties: {
    action: {}
    filterType: 'SqlFilter'
    sqlFilter: {
      compatibilityLevel: 20
      sqlExpression: '1=1'
    }
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_getcarts_get_carts_Default 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_carts_contracts_getcarts_get_carts
  location: location
  name: '$Default'
  properties: {
    action: {}
    filterType: 'SqlFilter'
    sqlFilter: {
      compatibilityLevel: 20
      sqlExpression: '1=1'
    }
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_removecartitem_remove_cart_item_quantity_Default 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_carts_contracts_removecartitem_remove_cart_item_quantity
  location: location
  name: '$Default'
  properties: {
    action: {}
    filterType: 'SqlFilter'
    sqlFilter: {
      compatibilityLevel: 20
      sqlExpression: '1=1'
    }
  }
}

resource namespaces_yourbrand_servicebus_name_carts_contracts_updatecartitemquantity_update_cart_item_quantity_Default 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_carts_contracts_updatecartitemquantity_update_cart_item_quantity
  location: location
  name: '$Default'
  properties: {
    action: {}
    filterType: 'SqlFilter'
    sqlFilter: {
      compatibilityLevel: 20
      sqlExpression: '1=1'
    }
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_productdetailsupdated_product_details_updated_Default 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_catalog_contracts_productdetailsupdated_product_details_updated
  location: location
  name: '$Default'
  properties: {
    action: {}
    filterType: 'SqlFilter'
    sqlFilter: {
      compatibilityLevel: 20
      sqlExpression: '1=1'
    }
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_producthandleupdated_product_handle_updated_Default 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_catalog_contracts_producthandleupdated_product_handle_updated
  location: location
  name: '$Default'
  properties: {
    action: {}
    filterType: 'SqlFilter'
    sqlFilter: {
      compatibilityLevel: 20
      sqlExpression: '1=1'
    }
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_productimageupdated_product_image_updated_Default 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_catalog_contracts_productimageupdated_product_image_updated
  location: location
  name: '$Default'
  properties: {
    action: {}
    filterType: 'SqlFilter'
    sqlFilter: {
      compatibilityLevel: 20
      sqlExpression: '1=1'
    }
  }
}

resource namespaces_yourbrand_servicebus_name_catalog_contracts_productpriceupdated_product_price_updated_Default 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2022-10-01-preview' = {
  parent: namespaces_yourbrand_servicebus_name_catalog_contracts_productpriceupdated_product_price_updated
  location: location
  name: '$Default'
  properties: {
    action: {}
    filterType: 'SqlFilter'
    sqlFilter: {
      compatibilityLevel: 20
      sqlExpression: '1=1'
    }
  }
}
