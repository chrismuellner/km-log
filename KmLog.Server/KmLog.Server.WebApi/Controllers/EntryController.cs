using System.Threading.Tasks;
using KmLog.Server.Domain;
using KmLog.Server.Dto;
using KmLog.Server.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KmLog.Server.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EntryController : ControllerBase
    {
        private readonly EntryLogic _entryLogic;

        public EntryController(EntryLogic refuelEntryLogic)
        {
            _entryLogic = refuelEntryLogic;
        }

        [HttpGet("refuel/{licensePlate}")]
        public async Task<IActionResult> LoadRefuelsPaged(string licensePlate, [FromQuery] PagingParameters pagingParameters)
        {
            var result = await _entryLogic.LoadRefuelsPaged(pagingParameters, licensePlate);
            return Ok(result);
        }

        [HttpGet("service/{licensePlate}")]
        public async Task<IActionResult> LoadServicesPaged(string licensePlate, [FromQuery] PagingParameters pagingParameters)
        {
            var result = await _entryLogic.LoadServicesPaged(pagingParameters, licensePlate);
            return Ok(result);
        }

        [HttpGet("refuel/{licensePlate}/latest")]
        public async Task<IActionResult> LoadLatestRefuel(string licensePlate)
        {
            var result = await _entryLogic.LoadLatestRefuel(licensePlate);
            return Ok(result);
        }

        [HttpGet("service/{licensePlate}/latest")]
        public async Task<IActionResult> LoadLatestService(string licensePlate)
        {
            var result = await _entryLogic.LoadLatestService(licensePlate);
            return Ok(result);
        }

        [HttpPut("refuel")]
        public async Task<IActionResult> AddRefuel([FromBody] RefuelEntryDto refuelEntry)
        {
            var added = await _entryLogic.AddRefuel(refuelEntry);
            if (added == null)
            {
                return BadRequest();
            }
            return Ok(added);
        }

        [HttpPut("service")]
        public async Task<IActionResult> AddService([FromBody] ServiceEntryDto serviceEntry)
        {
            var added = await _entryLogic.AddService(serviceEntry);
            if (added == null)
            {
                return BadRequest();
            }
            return Ok(added);
        }
    }
}
