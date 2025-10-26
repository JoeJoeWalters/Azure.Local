:: Run clean first
call clean.bat

:: Because the cosmos image only lasts for 90 day you have to remove it and repull if you want to refresh the licence for it
docker image rm mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator
docker pull mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:latest