using Azure.Provisioning.CosmosDB;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

#pragma warning disable ASPIRECOSMOSDB001

var cosmos = builder.AddAzureCosmosDB("cosmosdb").RunAsEmulator(
                    emulator =>
                    {
                        var endpoint = emulator.GetEndpoint("emulator");

                        emulator
                            //.WithDataExplorer()
                            .WithLifetime(ContainerLifetime.Session)
                            //.WithEnvironment("AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE", "127.0.0.1")
                            .WithHttpEndpoint(port: endpoint.TargetPort, targetPort: endpoint.TargetPort, isProxied: false); // https://github.com/dotnet/aspire/issues/6349
                    });

var storage = builder.AddAzureStorage("storageaccount").RunAsEmulator(
                    emulator =>
                        {
                            emulator
                                .WithLifetime(ContainerLifetime.Session)
                                .WithDataVolume();
                        }
                    );

//var orders = cosmos.AddCosmosDatabase("orders");
//var details = orders.AddContainer("details", "/id");

var apiService = builder.AddProject<Projects.Azure_Local_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Azure_Local_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(cosmos)
    .WaitFor(cosmos)
    .WaitFor(storage)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
