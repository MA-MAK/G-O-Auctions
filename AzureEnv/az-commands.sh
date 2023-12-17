#!/bin/bash

az login
az account set --subscription 0f413329-ae12-44cc-bb49-0e5c93f4dcad

ResourceGroup=GOAuctionsProdRG

az group create --name $ResourceGroup --location eastus --verbose

az deployment group create --resource-group $ResourceGroup --template-file GOAuctions.bicep --subscription 0f413329-ae12-44cc-bb49-0e5c93f4dcad --debug 
#az deployment group create --resource-group $ResourceGroup --template-file GOAuctionsServices.bicep --verbose
#az resource list --resource-group $ResourceGroup
