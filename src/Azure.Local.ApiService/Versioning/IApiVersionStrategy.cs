namespace Azure.Local.ApiService.Versioning
{
    public interface IApiVersionStrategy
    {
        string GetRequestedVersion(HttpContext httpContext);
    }
}
