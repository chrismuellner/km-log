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
    public class RefuelEntryController : ControllerBase
    {
        private readonly ILogger<RefuelEntryController> _logger;
        private readonly RefuelEntryLogic _refuelEntryLogic;

        public RefuelEntryController(ILogger<RefuelEntryController> logger, RefuelEntryLogic refuelEntryLogic)
        {
            _logger = logger;
            _refuelEntryLogic = refuelEntryLogic;
        }

        [HttpGet("id/{carId}")]
        public async Task<IActionResult> LoadByCarId(string carId)
        {
            if (!Guid.TryParse(carId, out var carGuid))
            {
                return BadRequest();
            }

            var fuelActions = await _refuelEntryLogic.LoadByCarId(carGuid);
            return Ok(fuelActions);
        }

        [HttpGet("{licensePlate}")]
        public async Task<IActionResult> LoadPaged(string licensePlate, [FromQuery] PagingParameters pagingParameters)
        {
            var result = await _refuelEntryLogic.LoadPaged(pagingParameters, licensePlate);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] RefuelEntryDto refuelEntry)
        {
            if (refuelEntry.CarId == Guid.Empty)
            {
                return BadRequest();
            }

            var added = await _refuelEntryLogic.Add(refuelEntry);
            return Ok(added);
        }
    }
}
