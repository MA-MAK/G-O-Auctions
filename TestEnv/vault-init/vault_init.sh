#!/bin/bash

export VAULT_ADDR='http://GOvault:8200'
export VAULT_TOKEN='00000000-0000-0000-0000-000000000000'

# give some time for Vault to start and be ready
sleep 5

# Insert 2 secrets  
vault kv put auctionSecrets/secret Secret=fwnhy8423HBgbirfffwefefwefwefwedqwsad6q3wfrhgedr32etsg7u

