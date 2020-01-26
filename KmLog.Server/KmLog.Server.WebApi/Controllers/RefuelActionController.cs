using System;
using System.Threading.Tasks;
using KmLog.Server.Dto;
using KmLog.Server.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RefuelActionController : ControllerBase
    {
        private readonly ILogger<RefuelActionController> _logger;
        private readonly FuelActionLogic _fuelActionLogic;

        public RefuelActionController(ILogger<RefuelActionController> logger, FuelActionLogic fuelActionLogic)
        {
            _logger = logger;
            _fuelActionLogic = fuelActionLogic;
        }

        [HttpGet]
        public async Task<IActionResult> LoadAll()
        {
            var fuelActions = await _fuelActionLogic.LoadAll();
            return Ok(fuelActions);
        }

        [HttpPut("{carId}")]
        public async Task<IActionResult> Add([FromQuery] Guid carId, [FromBody] RefuelActionDto refuelAction)
        {
            var added = await _fuelActionLogic.Add(carId, refuelAction);
            return Ok(added);
        }

        [HttpPost("{carId}/csv")]
        public async Task<IActionResult> UploadCsv(string carId, IFormFile csvFile)
        {
            if (!Guid.TryParse(carId, out var carGuid))
            {
                return BadRequest();
            }

            var fileStream = csvFile.OpenReadStream();
            var fileName = csvFile.FileName;

            var refuelActions = await _fuelActionLogic.ImportCsv(carGuid, fileStream, fileName);
            return Ok(refuelActions);
        }
    }
}
