using System.Linq;
using System.Threading.Tasks;
using KmLog.Server.Dto;
using KmLog.Server.Logic;
using KmLog.Server.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var email = User.GetEmail();

            var cars = await _carLogic.LoadByUser(email);
            return Ok(cars);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] CarDto car)
        {
            var email = User.GetEmail();

            var added = await _carLogic.Add(car, email);
            if (added == null)
            {
                return NotFound();
            }

            return Ok(added);
        }

        [HttpGet("{licensePlate}")]
        public async Task<IActionResult> LoadStatistics(string licensePlate)
        {
            var statistics = await _carLogic.LoadStatistics(licensePlate);

            return Ok(statistics);
        }

        [HttpPost("csv")]
        public async Task<IActionResult> UploadCsv()
        {
            var form = HttpContext.Request.Form;
            if (form.Files.Any())
            {
                var file = form.Files.First();

                var formDict = form.ToDictionary(g => g.Key, g => g.Value.ToString());

                using var fileStream = file.OpenReadStream();

                var email = User.GetEmail();
                var refuelEntries = await _carLogic.ImportCsv(email, fileStream, formDict);
                if (refuelEntries == null)
                {
                    return NotFound();
                }
                return Ok(refuelEntries);
            }
            return BadRequest();
        }
    }
}
