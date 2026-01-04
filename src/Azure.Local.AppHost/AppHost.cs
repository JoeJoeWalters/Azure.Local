var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var cosmos = builder.AddAzureCosmosDB("cosmosdb").RunAsEmulator(
                    emulator =>
                    {
                        var endpoint = emulator.GetEndpoint("emulator");

                        emulator
                            .WithLifetime(ContainerLifetime.Session)
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

var servicebus = builder.AddAzureServiceBus("servicebus").RunAsEmulator();


var apiService = builder.AddProject<Projects.Azure_Local_ApiService>("apiservice")
    .WithReference(cosmos)
    .WaitFor(cosmos)
    .WithHttpHealthCheck("/health");

var functionApp = builder.AddAzureFunctionsProject<Projects.Azure_Local_Functions>("functionapp")
    .WithReference(servicebus)
    .WaitFor(servicebus)
    .WithReference(cosmos)
    .WaitFor(cosmos);

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
