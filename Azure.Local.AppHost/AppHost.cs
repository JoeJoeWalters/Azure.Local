var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var cosmos = builder.AddAzureCosmosDB("cosmosdb").RunAsEmulator(
                    emulator =>
                    {
                        emulator
                        .WithLifetime(ContainerLifetime.Session)
                        .WithPartitionCount(2);
                        //.WithDataVolume();
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
