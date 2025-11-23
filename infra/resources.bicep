targetScope = 'resourceGroup'

param location string = resourceGroup().location
param appServiceName string
param skuName string

resource appService 'Microsoft.Web/sites@2021-02-01' = {
  name: appServiceName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
  }
}

resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: '${appServiceName}-plan'
  location: location
  sku: {
    name: skuName
    tier: 'Free'
  }
}

output appServiceEndpoint string = appService.properties.defaultHostName
