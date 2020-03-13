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

        [HttpGet("{licensePlate}")]
        public async Task<IActionResult> LoadPaged(string licensePlate, [FromQuery] PagingParameters pagingParameters)
        {
            var result = await _refuelEntryLogic.LoadPaged(pagingParameters, licensePlate);
            return Ok(result);
        }

        [HttpGet("{licensePlate}/latest")]
        public async Task<IActionResult> LoadLatest(string licensePlate)
        {
            var result = await _refuelEntryLogic.LoadLatest(licensePlate);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] RefuelEntryDto refuelEntry)
        {
            if (refuelEntry.CarId == Guid.Empty 
             || refuelEntry.Distance == 0 
             || refuelEntry.TotalDistance == 0)
            {
                return BadRequest();
            }

            var added = await _refuelEntryLogic.Add(refuelEntry);
            return Ok(added);
        }
    }
}
