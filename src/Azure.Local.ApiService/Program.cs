using Azure.Local.Application;
using Azure.Local.Infrastructure;
using Azure.Local.Infrastructure.Repository;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]

// Required to enable In-Memory component testing
public partial class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add service defaults & Aspire client integrations.
        builder.AddServiceDefaults();

        // Set up dependent api services
        builder.Services
            .AddApplication(builder.Configuration)
            .AddInfrastructure(builder.Configuration);

        // Add services to the container.
        builder.Services.AddProblemDetails();

        // Register MVC controllers
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // Map controller endpoints
        app.MapControllers();

        app.MapDefaultEndpoints();

        app.Run();
    }
}