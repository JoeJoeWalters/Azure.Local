using Asp.Versioning;
using System.Diagnostics.CodeAnalysis;

namespace Azure.Local.ApiService.Versioning
{
    [ExcludeFromCodeCoverage]
    public static class ServiceExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddHeaderApiVersioning()
            {
                services.AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = ApiVersioningConstants.DefaultVersion;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new HeaderApiVersionReader(ApiVersioningConstants.HeaderName);
                })
                .AddMvc();

                services.AddSingleton<IApiVersionStrategy, ApiVersionStrategy>();

                return services;
            }
        }
    }
}
