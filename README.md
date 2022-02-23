# Thaudal API
Backend API for the Thaudal.com website.

## Setup
### Cosmos Emulator Container
Run following command to start the Cosmos emulator container:
docker run -p 8081:8081 -p 10251:10251 -p 10252:10252 -p 10253:10253 -p 10254:10254  -m 3g --cpus=2.0 --name=azure-cosmos-emulator -e AZURE_COSMOS_EMULATOR_PARTITION_COUNT=5 -e AZURE_COSMOS_EMULATOR_ENABLE_DATA_PERSISTENCE=true -e AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE=127.0.0.1 -it mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator

After download and trust the certificate: curl -k https://localhost:8081/_explorer/emulator.pem > emulatorcert.crt

After this the Cosmos emulator should be available at https://localhost:8081/_explorer/index.html

The service depends on a database named "ThaudalAPI" and a container named "Users" (configurable in appsettings).