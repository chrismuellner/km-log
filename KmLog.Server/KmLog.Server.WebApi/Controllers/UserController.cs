using System.Threading.Tasks;
using KmLog.Server.Dto;
using KmLog.Server.Logic;
using KmLog.Server.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserLogic _userLogic;

        public UserController(ILogger<UserController> logger, UserLogic userLogic)
        {
            _logger = logger;
            _userLogic = userLogic;
        }

        [HttpGet]
        public async Task<IActionResult> LoadUser()
        {
            var email = User.GetEmail();

            var user = await _userLogic.LoadUser(email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut("group")]
        public async Task<IActionResult> Add([FromBody] GroupDto group)
        {
            var added = await _userLogic.AddGroup(group);
            if (added == null)
            {
                return NotFound();
            }

            return Ok(added);
        }

        [HttpGet("group")]
        public async Task<IActionResult> LoadGroups()
        {
            var groups = await _userLogic.LoadGroups();
            if (groups == null)
            {
                return NoContent();
            }

            return Ok(groups);
        }

        [HttpPost("group")]
        public async Task<IActionResult> Join([FromBody] GroupDto group)
        {
            var email = User.GetEmail();

            var joined = await _userLogic.JoinGroup(group, email);
            if (joined)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
