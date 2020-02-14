using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
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
            var email = GetEmail(User.Identity);

            var cars = await _carLogic.LoadByUser(email);
            return Ok(cars);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] CarDto car)
        {
            var added = await _carLogic.Add(car);
            return Ok(added);
        }

        [HttpPost("csv")]
        public async Task<IActionResult> UploadCsv()
        {
            if (HttpContext.Request.Form.Files.Any())
            {
                var file = HttpContext.Request.Form.Files.First();
                var fileStream = file.OpenReadStream();
                var fileName = file.FileName;

                var email = GetEmail(User.Identity);

                var refuelActions = await _carLogic.ImportCsv(email, fileStream, fileName);
                if (refuelActions == null)
                {
                    return NotFound();
                }
                return Ok(refuelActions);
            }
            return BadRequest();
        }

        private string GetEmail(IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
            return email;
        }
    }
}
