using Microsoft.AspNetCore.Mvc;

namespace Azure.Local.ApiService.Health.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeartbeatController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // Always returns 200 OK
            return Ok();
        }
    }
}