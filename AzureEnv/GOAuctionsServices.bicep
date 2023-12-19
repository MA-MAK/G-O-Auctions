@description('Location for all resources.')
param location string = resourceGroup().location

@description('Container images to deploy. Should be of the form repoName/imagename:tag for images stored in public Docker Hub, or a fully qualified URI for other registries. Images from private registries require additional registry credentials.')
param AssessmentServiceImage string = 'asnielsen789/assessmentservice:latest'
param AuctionServiceImage string = 'asnielsen789/auctionservice:latest'
param BidServiceImage string = 'asnielsen789/bidservice:latest'
param CustomerServiceImage string = 'asnielsen789/customerservice:latest'
param ItemServiceImage string = 'asnielsen789/itemservice:latest'
param LegalServiceImage string = 'asnielsen789/legalservice:latest'
param SaleServiceImage string = 'asnielsen789/saleservice:latest'
param BidWorkerImage string = 'asnielsen789/bidworker:latest'
/*
param LokiEndpoint string='http://backend:3100'
param ConnectionString string='mongodb://admin:1234@backend:27017/?authSource=admin'
param DatabaseName string='Auction'
param jwtSecret string='fwnhy8423HBgbirfffwefefwefwefwedqwsad6q3wfrhgedr32etsg7u'
param jwtIssuer string='MLSAuction'
param Salt string='$2a$11$NnQ3D9KHpPr1UOjTo/2fXO'
param MqHost string='backend'
*/
param vnetname string = 'goauctionsVNet'
param subnetName string = 'goServicesSubnet'
param dnsRecordName string = 'SERVICES'
param dnszonename string = 'goauctions.dk'
param storageAccountName string = 'storage2cwvexx63utd4'

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
        name: 'assessmentservice'
        properties: {
          image: AssessmentServiceImage
          ports: [
            {
              port: 5000
            }
          ]
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://GOUser:qwer1234@backend:27017'
            }
            {
              name: 'databaseName'
              value: 'GODatabase'
            }
            {
              name: 'collectionName'
              value: 'assessments'
            }
            {
              name: 'ItemService'
              value: 'http://localhost:5004'
            }
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://localhost:5000'
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
        name: 'auctionservice'
        properties: {
          image: AuctionServiceImage
          ports: [
            {
              port: 5001
            }
          ]
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://GOUser:qwer1234@backend:27017'
            }
            {
              name: 'databaseName'
              value: 'GODatabase'
            }
            {
              name: 'collectionName'
              value: 'auctions'
            }
            {
              name: 'BidService'
              value: 'http://localhost:5002'
            }
            {
              name: 'ItemService'
              value: 'http://localhost:5004'
            }
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://localhost:5001'
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
        name: 'bidservice'
        properties: {
          image: BidServiceImage
          ports: [
            {
              port: 5002
            }
          ]
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://GOUser:qwer1234@backend:27017'
            }
            {
              name: 'databaseName'
              value: 'GODatabase'
            }
            {
              name: 'collectionName'
              value: 'bids'
            }
            {
              name: 'CustomerService'
              value: 'http://localhost:5003'
            }
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://localhost:5002'
            }
            {
              name: 'rabbitmq'
              value: 'amqp://guest:guest@backend:5672'
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
              port: 5003
            }
          ]
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://GOUser:qwer1234@backend:27017'
            }
            {
              name: 'databaseName'
              value: 'GODatabase'
            }
            {
              name: 'collectionName'
              value: 'Customers'
            }
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://localhost:5003'
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
          ports: [
            {
              port: 5004
            }
          ]
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://GOUser:qwer1234@backend:27017'
            }
            {
              name: 'databaseName'
              value: 'GODatabase'
            }
            {
              name: 'collectionName'
              value: 'items'
            }
            {
              name: 'CustomerService'
              value: 'http://localhost:5003'
            }
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://localhost:5004'
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
        name: 'legalservice'
        properties: {
          image: LegalServiceImage
          ports: [
            {
              port: 5005
            }
          ]
          environmentVariables: [
            {
              name: 'AuctionService'
              value: 'http://localhost:5001'
            }
            {
              name: 'CustomerService'
              value: 'http://localhost:5003'
            }
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://localhost:5005'
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
        name: 'saleservice'
        properties: {
          image: SaleServiceImage
          ports: [
            {
              port: 5006
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
              value: 'sales'
            }
            {
              name: 'AuctionService'
              value: 'http://localhost:5001'
            }
            {
              name: 'CustomerService'
              value: 'http://localhost:5003'
            }
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://localhost:5006'
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
        name: 'bidworker'
        properties: {
          image: BidWorkerImage
          environmentVariables: [
            {
              name: 'connectionString'
              value: 'mongodb://GOUser:qwer1234@backend:27017'
            }
            {
              name: 'databaseName'
              value: 'GODatabase'
            }
            {
              name: 'collectionName'
              value: 'bids'
            }
            {
              name: 'rabbitmq'
              value: 'amqp://guest:guest@backend:5672'
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
        name: 'nginx'
        properties: {
          image: 'nginx:latest'
          ports: [
            {
              port: 80
            }
          ]
          environmentVariables: []
          resources: {
            requests: {
              memoryInGB: json('0.5')
              cpu: json('0.5')
            }
          }
          volumeMounts: [
            {
              name: 'nginx-config'
              mountPath: '/etc/nginx/'
            }
          ]
        }
      }
    ]
    initContainers: []
    restartPolicy: 'Always'
    ipAddress: {
      ports: [
        {
          port: 80
        }
      ]
      type: 'private'
      ip: '10.0.3.4'

    }
    osType: 'Linux'
    volumes: [
      {
        name: 'nginx-config'
        azureFile: {
          shareName: 'storageconfig'
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


/*command: [
            'tail'
            '-f'
            '/dev/null'
          ]*/
