namespace Azure.Local.ApiService.Versioning
{
    public class ApiVersionStrategy : IApiVersionStrategy
    {
        public string GetRequestedVersion(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.TryGetValue(ApiVersioningConstants.HeaderName, out var values))
                return ApiVersioningConstants.V1;

            var version = values.ToString();
            if (string.IsNullOrWhiteSpace(version))
                return ApiVersioningConstants.V1;

            return version.Split(',')[0].Trim();
        }
    }
}
