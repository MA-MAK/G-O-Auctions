#!/bin/bash

RESGROUP=GOAuctionsRG
GATEWAYNAME=goauctionsAppGateway

echo "Starting Azure container groups ..."

az container start --name goAuctionsServicesGroup --resource-group $RESGROUP
az container start --name goAuctionsBackendGroup --resource-group $RESGROUP
az container start --name goAuctionsDevOpsGroup --resource-group $RESGROUP

echo "Starting Azure Application Gateway ..."

az network application-gateway start -g $RESGROUP -n $GATEWAYNAME
