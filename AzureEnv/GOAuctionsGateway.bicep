@description('Specifies the location for resources.')
param location string = 'eastus'

resource goauctionspublicip 'Microsoft.Network/publicIPAddresses@2022-11-01' = {
  name: 'goauctions-public_ip'
  location: location
  properties: {
    ipAddress: '20.119.63.169'
    publicIPAddressVersion: 'IPv4'
    publicIPAllocationMethod: 'Static'
    idleTimeoutInMinutes: 4
    dnsSettings: {
      domainNameLabel: 'goauctions'
      fqdn: 'goauctions.eastus.cloudapp.azure.com'
    }
    ipTags: []
    ddosSettings: {
      protectionMode: 'VirtualNetworkInherited'
    }
  }
  sku: {
    name: 'Standard'
    tier: 'Regional'
  }
}
