using System.Threading.Tasks;
using KmLog.Server.Dto;
using KmLog.Server.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ILogger<CarController> _logger;
        private readonly CarLogic _carLogic;

        public CarController(ILogger<CarController> logger, CarLogic carLogic)
        {
            _logger = logger;
            _carLogic = carLogic;
        }

        [HttpGet]
        public async Task<IActionResult> LoadAll()
        {
            var cars = await _carLogic.LoadAll();
            return Ok(cars);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] CarDto car)
        {
            var added = await _carLogic.Add(car);
            return Ok(added);
        }
    }
}
