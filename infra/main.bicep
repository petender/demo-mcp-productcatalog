targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string

// Tags that should be applied to all resources.
// 

// serviceName is used as value for the tag (azd-service-name) azd uses to identify deployment host
param serviceName string = 'web'
var tags = {
  'azd-env-name': environmentName
  SecurityControl: 'Ignore'
}

var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))

// This deploys the Resource Group
resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-${environmentName}'
  location: location
  tags: tags
  
}

module resources 'resources.bicep' = {
  name: 'resourcesModule'
  scope: rg
  params: {
    location: location
    appServiceName: '${resourceToken}app'
    skuName: 'F1'
    serviceTag: serviceName
    tags: tags
  }
}

output appServiceEndpoint string = resources.outputs.appServiceEndpoint
