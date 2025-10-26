:: Stop all of the docker containers
for /F %%i in ('docker ps -aq') do docker stop %%i

:: Remove all of the docker containers
for /F %%i in ('docker ps -aq') do docker rm -f %%i

:: Clean up any instances of Aspire sessions clogging up docker network that have not closed correctly by Aspire
:: "could not create the network as all available subnet ranges from the default pool are allocated"
docker network prune
