using System;
using System.Threading.Tasks;
using KmLog.Server.Domain;
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
    public class RefuelActionController : ControllerBase
    {
        private readonly ILogger<RefuelActionController> _logger;
        private readonly FuelActionLogic _fuelActionLogic;

        public RefuelActionController(ILogger<RefuelActionController> logger, FuelActionLogic fuelActionLogic)
        {
            _logger = logger;
            _fuelActionLogic = fuelActionLogic;
        }

        [HttpGet("id/{carId}")]
        public async Task<IActionResult> LoadByCarId(string carId)
        {
            if (!Guid.TryParse(carId, out var carGuid))
            {
                return BadRequest();
            }

            var fuelActions = await _fuelActionLogic.LoadByCarId(carGuid);
            return Ok(fuelActions);
        }

        [HttpGet("{licensePlate}")]
        public async Task<IActionResult> LoadPaged(string licensePlate, [FromQuery] PagingParameters pagingParameters)
        {
            var result = await _fuelActionLogic.LoadPaged(pagingParameters, licensePlate);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] RefuelActionDto refuelAction)
        {
            if (refuelAction.CarId == Guid.Empty)
            {
                return BadRequest();
            }

            var added = await _fuelActionLogic.Add(refuelAction);
            return Ok(added);
        }
    }
}
