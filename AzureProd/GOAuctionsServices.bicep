@description('Name for the container group')
param name string = 'GOAuctionsServices'

@description('Location for all resources.')
param location string = resourceGroup().location

@description('Container images to deploy. Should be of the form repoName/imagename:tag for images stored in public Docker Hub, or a fully qualified URI for other registries. Images from private registries require additional registry credentials.')
param AuctionServiceImage string = 'asnielsen789/auctionservice:latest'
param ItemServiceImage string = 'asnielsen789/itemservice:latest'
param BidServiceImage string = 'asnielsen789/bidservice:latest'
param CustomerServiceImage string = 'asnielsen789/customerservice:latest'
param AssessmentServiceImage string = 'asnielsen789/assessmentservice:latest'

@description('Port to open on the container and the public IP address.')
param port int = 80

@description('The number of CPU cores to allocate to the container.')
param cpuCores int = 1

@description('The amount of memory to allocate to the container in gigabytes.')
param memoryInGb int = 2

@description('The behavior of Azure runtime if container has stopped.')
@allowed([
  'Always'
  'Never'
  'OnFailure'
])
param restartPolicy string = 'Always'

@description('Specifies the name of the Azure Storage account.')
param storageAccountName string = 'storage${uniqueString(resourceGroup().id)}'

@description('Specifies the prefix of the file share names.')
param sharePrefix string = 'storage'

@description('List of file shares to create')
var shareNames = [
  'dbdata'
  'images'
]

//--- Opret storage konto til volumener ---
resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    accessTier: 'Hot'
  }
}

//-- Opret et share i storage account til MongoDB data og Image upload data
resource fileshare 'Microsoft.Storage/storageAccounts/fileServices/shares@2021-04-01' = [for share in shareNames : {
  name: '${storageAccount.name}/default/${sharePrefix}${share}'
}]


resource containerGroup 'Microsoft.ContainerInstance/containerGroups@2021-09-01' = {
  name: name
  location: location
  properties: {
    containers: [
      {
        name: name
        properties: {
          image: AuctionServiceImage
          environmentVariables: [
            {
              name: 'MongoConnectionString'
              value: 'mongodb://root:rootpassword@localhost:27017/?authSource=admin'
            }
            {
              name: 'CatalogDatabase'
              value: 'catalog'
            }
            {
              name: 'CatalogCollection'
              value: 'products'
            }
          ]
          ports: [
            {
              port: port
              protocol: 'TCP'
            }
          ]
          resources: {
            requests: {
              cpu: cpuCores
              memoryInGB: memoryInGb
            }
          }
          volumeMounts: [
            {
              name: 'images'
              mountPath: '/srv/resources/images'
            }
          ]
        }
      }
      {
        name: 'mongodb'
        properties: {
          image: 'mongo:latest'
          command: [
            'mongod'
            '--dbpath=/data/mongodb'
            '--auth'
            '--bind_ip_all'
          ]
          ports: [
            {
              port: 27017
            }
          ]
          environmentVariables: [
            {
              name: 'MONGO_INITDB_ROOT_USERNAME'
              value: 'root'
            }
            {
              name: 'MONGO_INITDB_ROOT_PASSWORD'
              value: 'rootpassword'
            }
          ]
          resources: {
            requests: {
              memoryInGB: json('0.5')
              cpu: json('0.5')
            }
          }
          volumeMounts: [
            {
              name: 'db'
              mountPath: '/data/mongodb/'
            }
          ]
        }
      }
    ]
    osType: 'Linux'
    volumes: [
      {
        name: 'db'
        azureFile: {
          shareName: 'storagedbdata'
          storageAccountName: storageAccount.name
          storageAccountKey: storageAccount.listKeys().keys[0].value
        }
      }
      {
        name: 'images'
        azureFile: {
          shareName: 'storageimages'
          storageAccountName: storageAccount.name
          storageAccountKey: storageAccount.listKeys().keys[0].value
        }
      }
    ]
    restartPolicy: restartPolicy
    ipAddress: {
      type: 'Public'
      ports: [
        {
          port: port
          protocol: 'TCP'
        }
      ]
    }
  }
}

output containerIPv4Address string = containerGroup.properties.ipAddress.ip
