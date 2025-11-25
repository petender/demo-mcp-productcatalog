targetScope = 'resourceGroup'

param location string = resourceGroup().location
param appServiceName string
param skuName string
param serviceTag string
param tags object = {}

resource appService 'Microsoft.Web/sites@2023-12-01' = {
  name: appServiceName
  location: location
  tags: union(tags, {
      'azd-service-name': serviceTag
    })

  properties: {
    serverFarmId: appServicePlan.id
    
  }
}

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: '${appServiceName}-plan'
  location: location
  tags: tags
  sku: {
    name: skuName
    tier: 'Free'
  }
}

output appServiceEndpoint string = appService.properties.defaultHostName
