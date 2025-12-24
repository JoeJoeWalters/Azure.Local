using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.ApiService.Timesheets.Controllers.Validators;
using Azure.Local.Application;
using Azure.Local.Infrastructure;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Azure.Local.ApiService
{
    [ExcludeFromCodeCoverage]

    // Required to enable In-Memory component testing
    public partial class Program
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        static async Task Main(string[] args)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add service defaults & Aspire client integrations.
            builder.AddServiceDefaults();

            // Set up dependent api services
            builder.Services
                .AddApplication(builder.Configuration)
                .AddInfrastructure(builder.Configuration);

            // Register validators
            builder.Services.AddScoped<IValidator<AddTimesheetHttpRequest>, AddTimesheetHttpRequestValidator>();
            builder.Services.AddScoped<IValidator<PatchTimesheetHttpRequest>, PatchTimesheetHttpRequestValidator>();

            // Add services to the container.
            builder.Services.AddProblemDetails();

            // Register MVC controllers
            builder.Services.AddControllers();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();/* (options => 
                options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0
            );*/

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "v1");
                });
            }

            // Map controller endpoints
            app.MapControllers();

            app.MapDefaultEndpoints();

            app.Run();
        }
    }
}