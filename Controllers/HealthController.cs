using Microsoft.AspNetCore.Mvc;

namespace taskmanager_back_repo_qas.Controllers
{
    [Route("health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { status = "healthy" });
    }
}
