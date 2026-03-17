using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Azure.Local.ApiService.Versioning
{
    [ExcludeFromCodeCoverage]
    public static class ApiVersioningConstants
    {
        public const string HeaderName = "api-version";
        public const string V1 = "1.0";

        public static readonly ApiVersion DefaultVersion = new(1, 0);
    }
}
