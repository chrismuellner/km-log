using System.Threading.Tasks;
using KmLog.Server.Logic;
using KmLog.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KmLogController : ControllerBase
    {
        private readonly ILogger<KmLogController> _logger;
        private readonly KmLogLogic _kmLogLogic;

        public KmLogController(ILogger<KmLogController> logger, KmLogLogic kmLogLogic)
        {
            _logger = logger;
            _kmLogLogic = kmLogLogic;
        }

        [HttpGet]
        public async Task<IActionResult> LoadAll()
        {
            var journeys = await _kmLogLogic.LoadAll();
            return Ok(journeys);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] Journey journey)
        {
            var added = await _kmLogLogic.Add(journey);
            if (added != null)
            {
                return Ok(added);
            }
            return Problem();
        }
    }
}
