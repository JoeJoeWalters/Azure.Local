using Azure.Local.ApiService.Versioning;

namespace Azure.Local.ApiService.Health.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersioningConstants.V1)]
    [Route("[controller]")]
    public class HeartbeatController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Get() => Ok();
    }
}
