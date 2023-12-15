@description('Location for all resources.')
param location string = resourceGroup().location

param vnetname string = 'goauctionsVNet'
param subnetName string = 'goDevopsSubnet'
param storageAccountName string = 'storageAccount'
param dnsRecordName string = 'backendhostname'
param dnszonename string = 'goauctions.dk'

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
              cpu: json('1.0')
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
        // {
        //   port: 27017
        // }
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

output containerIPAddressFqdn string = GOAuctionsBackendGroup.properties.ipAddress.ip
