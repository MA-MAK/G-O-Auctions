#!/bin/bash

RESGROUP=GOAuctionsProdRG
GATEWAYNAME=goauctionsAppGateway

az container stop --name goAuctionsServicesGroup --resource-group $RESGROUP
az container stop --name goAuctionsBackendGroup --resource-group $RESGROUP
az container stop --name goAuctionsDevOpsGroup --resource-group $RESGROUP
az network application-gateway stop -g $RESGROUP -n $GATEWAYNAME
