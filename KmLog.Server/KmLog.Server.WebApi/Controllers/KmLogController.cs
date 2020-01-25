using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KmLogController : ControllerBase
    {
        private readonly ILogger<KmLogController> _logger;

        public KmLogController(ILogger<KmLogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
