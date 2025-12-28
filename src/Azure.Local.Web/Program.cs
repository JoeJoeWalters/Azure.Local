namespace Azure.Local.Web
{
    using Azure.Local.ServiceDefaults;
    using Azure.Local.Web.Components;
    using System.Diagnostics.CodeAnalysis;

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
            builder.AddRedisOutputCache("cache");

            // Update HSTS settings to extend basic 30 days to 60 days with preload and subdomains.
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.UseOutputCache();

            app.MapStaticAssets();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapDefaultEndpoints();

            app.Run();
        }
    }
}

