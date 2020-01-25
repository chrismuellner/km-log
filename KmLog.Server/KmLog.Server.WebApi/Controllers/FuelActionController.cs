using System.Threading.Tasks;
using KmLog.Server.Dto;
using KmLog.Server.Logic;
using KmLog.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FuelActionController : ControllerBase
    {
        private readonly ILogger<FuelActionController> _logger;
        private readonly FuelActionLogic _fuelActionLogic;

        public FuelActionController(ILogger<FuelActionController> logger, FuelActionLogic fuelActionLogic)
        {
            _logger = logger;
            _fuelActionLogic = fuelActionLogic;
        }

        [HttpGet]
        public async Task<IActionResult> LoadAll()
        {
            var journeys = await _fuelActionLogic.LoadAll();
            return Ok(journeys);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] RefuelActionDto refuelAction)
        {
            var added = await _fuelActionLogic.Add(refuelAction);
            if (added != null)
            {
                return Ok(added);
            }
            return Problem();
        }
    }
}
