@description('Location for all resources.')
param location string = resourceGroup().location

param vnetname string = 'goauctionsVNet'
param subnetName string = 'goDevopsSubnet'
param storageAccountName string = 'storageAccount'
param dnsRecordName string = 'backendhostname'
param dnszonename string = 'goauctions.dk'

@description('Cosmos DB account name')
param accountName string = 'GOUser'

@description('The primary replica region for the Cosmos DB account.')
param primaryRegion string

@description('The secondary replica region for the Cosmos DB account.')
param secondaryRegion string

@description('Max stale requests. Required for BoundedStaleness. Valid ranges, Single Region: 10 to 2147483647. Multi Region: 100000 to 2147483647.')
@minValue(10)
@maxValue(2147483647)
param maxStalenessPrefix int = 100000

@description('Max lag time (seconds). Required for BoundedStaleness. Valid ranges, Single Region: 5 to 84600. Multi Region: 300 to 86400.')
@minValue(5)
@maxValue(86400)
param maxIntervalInSeconds int = 300

@description('The name for the Mongo DB database')
param databaseName string = 'GODatabase'

@description('Maximum autoscale throughput for the database shared with up to 25 collections')
@minValue(1000)
@maxValue(1000000)
param sharedAutoscaleMaxThroughput int = 1000
/*
@description('The name for the first Mongo DB collection')
param collection1Name string = 'Customers'
*/
@description('Maximum dedicated autoscale throughput for the orders collection')
@minValue(1000)
@maxValue(1000000)
param dedicatedAutoscaleMaxThroughput int = 1000

@allowed([
  'Eventual'
  'ConsistentPrefix'
  'Session'
  'BoundedStaleness'
  'Strong'
])
@description('The default consistency level of the Cosmos DB account.')
param defaultConsistencyLevel string = 'Eventual'

@allowed([
  '3.2'
  '3.6'
  '4.0'
  '4.2'
])
@description('Specifies the MongoDB server version to use.')
param serverVersion string = '4.2'

var consistencyPolicy = {
  Eventual: {
    defaultConsistencyLevel: 'Eventual'
  }
  ConsistentPrefix: {
    defaultConsistencyLevel: 'ConsistentPrefix'
  }
  Session: {
    defaultConsistencyLevel: 'Session'
  }
  BoundedStaleness: {
    defaultConsistencyLevel: 'BoundedStaleness'
    maxStalenessPrefix: maxStalenessPrefix
    maxIntervalInSeconds: maxIntervalInSeconds
  }
  Strong: {
    defaultConsistencyLevel: 'Strong'
  }
}
var locations = [
  {
    locationName: primaryRegion
    failoverPriority: 0
    isZoneRedundant: false
  }
  {
    locationName: secondaryRegion
    failoverPriority: 1
    isZoneRedundant: false
  }
]

/*
@description('The name of the key vault to be created.')
param vaultName string = 'goauctionsVault'

@description('The name of the key to be created.')
param keyName string = 'goauctionsKey'

@description('The SKU of the vault to be created.')
@allowed([
  'standard'
  'premium'
])
param skuName string = 'standard'

@description('The JsonWebKeyType of the key to be created.')
@allowed([
  'EC'
  'EC-HSM'
  'RSA'
  'RSA-HSM'
])
param keyType string = 'RSA'

@description('The permitted JSON web key operations of the key to be created.')
param keyOps array = []

@description('The size in bits of the key to be created.')
param keySize int = 2048

@description('The JsonWebKeyCurveName of the key to be created.')
@allowed([
  ''
  'P-256'
  'P-256K'
  'P-384'
  'P-521'
])
param curveName string = ''
*/

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

@description('GOAuctionsBackendGroup')
resource GOAuctionsBackendGroup 'Microsoft.ContainerInstance/containerGroups@2023-05-01' = {
  name: 'GOAuctionsBackendGroup'
  location: location
  properties: {
    sku: 'Standard'
    containers: [
      {
        name: 'mongodb'
        properties: {
          image: 'mongo:latest'
          command: [
            'mongod'
            '--dbpath=/data/GODatabase'
            '--auth'
            '--bind_ip_all'
          ]
          ports: [
            {
              port: 27017
            }
          ]
          environmentVariables: []
          resources: {
            requests: {
              memoryInGB: json('1.0')
              cpu: json('0.5')
            }
          }
          volumeMounts: [
            {
              name: 'db'
              mountPath: '/data/GODatabase/'
            }
          ]
        }
      }
      {
        name: 'rabbitmq'
        properties: {
          image: 'rabbitmq:management'
          command: [ 'tail', '-f', '/dev/null' ]
          ports: [
            {
              port: 15672
            }
            {
              port: 5672
            }
          ]
          environmentVariables: []
          resources: {
            requests: {
              memoryInGB: json('1.0')
              cpu: json('0.5')
            }
          }
          volumeMounts: [
            {
              name: 'msgqueue'
              mountPath: '/var/lib/rabbitmq'
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
          port: 27017
        }
        {
          port: 15672
        }
        {
          port: 5672
        }
      ]
      ip: '10.0.1.4'
      type: 'Private'
    }
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
      {
        name: 'msgqueue'
        azureFile: {
          shareName: 'storagequeue'
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
        ipv4Address: GOAuctionsBackendGroup.properties.ipAddress.ip
      }
    ]
  }
}
/*
resource vault 'Microsoft.KeyVault/vaults@2021-11-01-preview' = {
  name: vaultName
  location: location
  properties: {
    accessPolicies:[]
    enableRbacAuthorization: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: false
    tenantId: subscription().tenantId
    sku: {
      name: skuName
      family: 'A'
    }
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
    }
  }
}

resource key 'Microsoft.KeyVault/vaults/keys@2021-11-01-preview' = {
  parent: vault
  name: keyName
  properties: {
    kty: keyType
    keyOps: keyOps
    keySize: keySize
    curveName: curveName
  }
}

output proxyKey object = key.properties
*/

resource account 'Microsoft.DocumentDB/databaseAccounts@2022-05-15' = {
  name: accountName
  location: location
  kind: 'MongoDB'
  properties: {
    consistencyPolicy: consistencyPolicy[defaultConsistencyLevel]
    locations: locations
    databaseAccountOfferType: 'Standard'
    enableAutomaticFailover: true
    apiProperties: {
      serverVersion: serverVersion
    }
    capabilities: [
      {
        name: 'DisableRateLimitingResponses'
      }
    ]
  }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/mongodbDatabases@2022-05-15' = {
  parent: account
  name: databaseName
  properties: {
    resource: {
      id: databaseName
    }
    options: {
      autoscaleSettings: {
        maxThroughput: sharedAutoscaleMaxThroughput
      }
    }
  }
}

output containerIPAddressFqdn string = GOAuctionsBackendGroup.properties.ipAddress.ip
