@description('Location for all resources.')
param location string = resourceGroup().location

param vnetname string = 'goauctionsVNet'
param subnetName string = 'goServicesSubnet'
param storageAccountName string = 'storage23bame2qqs4ac'
param dnsRecordName string = 'SERVICES'
param dnszonename string = 'goauctions.dk'

@description('Container images to deploy. Should be of the form repoName/imagename:tag for images stored in public Docker Hub, or a fully qualified URI for other registries. Images from private registries require additional registry credentials.')
param AuctionServiceImage string = 'asnielsen789/auctionservice:latest'
param ItemServiceImage string = 'asnielsen789/itemservice:latest'
param BidServiceImage string = 'asnielsen789/bidservice:latest'
param CustomerServiceImage string = 'asnielsen789/customerservice:latest'
param AssessmentServiceImage string = 'asnielsen789/assessmentservice:latest'
param SaleServiceImage string = 'asnielsen789/saleservice:latest'

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

resource GOAuctionsServicesGroup 'Microsoft.ContainerInstance/containerGroups@2021-09-01' = {
  name: 'GOAuctionsServicesGroup'
  location: location
  properties: {
    containers: [
      /*{
        name: 'auctionservice'
        properties: {
          image: AuctionServiceImage
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://admin:1234@mongodb-dev:27017'
            }
            {
              name: 'databaseName'
              value: 'GODB'
            }
            {
              name: 'collectionName'
              value: 'auctions'
            }
          ]
          ports: [
            {
              port: 5001
              protocol: 'TCP'
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
      /*{
        name: 'bidservice'
        properties: {
          image: BidServiceImage
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://admin:1234@mongodb-dev:27017'
            }
            {
              name: 'databaseName'
              value: 'GODB'
            }
            {
              name: 'collectionName'
              value: 'bids'
            }
          ]
          ports: [
            {
              port: 5002
              protocol: 'TCP'
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
        name: 'itemservice'
        properties: {
          image: ItemServiceImage
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://admin:1234@mongodb-dev:27017'
            }
            {
              name: 'databaseName'
              value: 'GODB'
            }
            {
              name: 'collectionName'
              value: 'items'
            }
          ]
          ports: [
            {
              port: 5003
              protocol: 'TCP'
            }
          ]
          resources: {
            requests: {
              memoryInGB: json('0.2')
              cpu: json('0.2')
            }
          }
        }
      }*/
      {
        name: 'customerservice'
        properties: {
          image: CustomerServiceImage
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://admin:1234@mongodb-dev:27017'
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
          ports: [
            {
              port: 5004
              protocol: 'TCP'
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
      /*{
        name: 'saleservice'
        properties: {
          image: SaleServiceImage
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://admin:1234@mongodb-dev:27017'
            }
            {
              name: 'databaseName'
              value: 'GODB'
            }
            {
              name: 'collectionName'
              value: 'sales'
            }
          ]
          ports: [
            {
              port: 5005
              protocol: 'TCP'
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
      /*{
        name: 'assessmentservice'
        properties: {
          image: AssessmentServiceImage
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://admin:1234@mongodb-dev:27017'
            }
            {
              name: 'databaseName'
              value: 'GODB'
            }
            {
              name: 'collectionName'
              value: 'assessments'
            }
          ]
          ports: [
            {
              port: 5006
              protocol: 'TCP'
            }
          ]
          resources: {
            requests: {
              memoryInGB: json('0.2')
              cpu: json('0.2')
            }
          }
        }
      }*/
    ]
    osType: 'Linux'
    volumes: [
      {
        name: 'db'
        azureFile: {
          shareName: 'storagedata'
          storageAccountName: storageAccount.name
          storageAccountKey: storageAccount.listKeys().keys[0].value
        }
      }
    ]
    restartPolicy: 'Always'
    ipAddress: {
      type: 'Public'
      ip: '10.0.3.4'
      ports: [
        /*{
          port: 5001
        }
        /*{
          port: 5002
        }
        {
          port: 5003
        }*/
        {
          port: 5004
        }
        /*{
          port: 5005
        }
        /*{
          port: 5006
        }*/
      ]
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
