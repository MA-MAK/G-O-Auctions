@description('Location for all resources.')
param location string = resourceGroup().location

@description('Container images to deploy. Should be of the form repoName/imagename:tag for images stored in public Docker Hub, or a fully qualified URI for other registries. Images from private registries require additional registry credentials.')
param AuctionServiceImage string = 'asnielsen789/auctionservice:latest'
param ItemServiceImage string = 'asnielsen789/itemservice:latest'
param BidServiceImage string = 'asnielsen789/bidservice:latest'
param CustomerServiceImage string = 'asnielsen789/customerservice:latest'
param AssessmentServiceImage string = 'asnielsen789/assessmentservice:latest'
param SaleServiceImage string = 'asnielsen789/saleservice:latest'

param vnetname string = 'goauctionsVNet'
param subnetName string = 'goServicesSubnet'
param dnsRecordName string = 'SERVICES'
param dnszonename string = 'goauctions.dk'
param storageAccountName string = 'nostorage'

resource VNET 'Microsoft.Network/virtualNetworks@2020-11-01' existing = {
  name: vnetname
  resource subnet 'subnets@2022-01-01' existing = {
    name: subnetName
  }
}

// Get a reference to the existing storage
resource storageAccount 'Microsoft.Storage/storageAccounts@2021-09-01' existing = {
  name: storageAccountName
}

resource GOAuctionsServicesGroup 'Microsoft.ContainerInstance/containerGroups@2023-05-01' = {
  name: 'GOAuctionsServicesGroup'
  location: location
  properties: {
    sku: 'Standard'
    containers: [
      {
        name: 'itemservice'
        properties: {
          image: ItemServiceImage
          ports: [
            {
              port: 5164
            }
          ]
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://GOUser:qwer1234@mongodb:27017'
            }
            {
              name: 'databaseName'
              value: 'GODatabase'
            }
            {
              name: 'collectionName'
              value: 'items'
            }
          ]
          resources: {
            requests: {
              memoryInGB: json('0.2')
              cpu: json('0.2')
            }
          }
        }
      }
      {
        name: 'customerservice'
        properties: {
          image: CustomerServiceImage
          ports: [
            {
              port: 5004
            }
          ]
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://GOUser:qwer1234@mongodb:27017'
            }
            {
              name: 'databaseName'
              value: 'GODatabase'
            }
            {
              name: 'collectionName'
              value: 'customers'
            }
          ]
          resources: {
            requests: {
              memoryInGB: json('0.2')
              cpu: json('0.2')
            }
          }
        }
      }
    ]
    initContainers: []
    restartPolicy: 'Always'
    ipAddress: {
      ports: [
        {
          port: 5004
        }
        {
          port: 5164
        }
      ]
      type: 'private'
      ip: '10.0.3.4'
    }
    osType: 'Linux'
    volumes: [
      {
        name: 'services-storage'
        azureFile: {
          shareName: 'storageservices'
          storageAccountName: storageAccount.name
          storageAccountKey: storageAccount.listKeys().keys[0].value
        }
      }
    ]
    subnetIds: [
      {
        id: VNET::subnet.id
      }
    ]
    dnsConfig: {
      nameServers: [
        '10.0.0.10'
        '10.0.0.11'
      ]
      searchDomains: dnszonename
    }
  }
}

resource dnsZone 'Microsoft.Network/privateDnsZones@2020-06-01' existing = {
  name: dnszonename
}

resource dnsRecord 'Microsoft.Network/privateDnsZones/A@2020-06-01' = {
  name: dnsRecordName
  parent: dnsZone
  properties: {
    ttl: 3600
    aRecords: [
      {
        ipv4Address: GOAuctionsServicesGroup.properties.ipAddress.ip
      }
    ]
  }
}

output containerIPAddressFqdn string = GOAuctionsServicesGroup.properties.ipAddress.ip
