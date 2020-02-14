using System.Security.Claims;
using System.Threading.Tasks;
using KmLog.Server.Domain;
using KmLog.Server.Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly AuthenticationLogic _authenticationLogic;

        private static readonly UserInfo LoggedOutUser = new UserInfo { IsAuthenticated = false };

        public AuthenticationController(ILogger<AuthenticationController> logger, AuthenticationLogic authenticationLogic)
        {
            _logger = logger;
            _authenticationLogic = authenticationLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity.IsAuthenticated)
            {
                var email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;

                if (email != null)
                {
                    await _authenticationLogic.CreateUserIfNew(email);
                    _logger.LogInformation($"Accessed user with email '{email}'.");
                } 
                else
                {
                    return BadRequest();
                }

                var user = new UserInfo
                {
                    Name = claimsIdentity.Name,
                    IsAuthenticated = true
                };
                return Ok(user);
            }
            return Ok(LoggedOutUser);
        }

        [HttpGet("signin")]
        public async Task SignIn(string redirectUri)
        {
            if (string.IsNullOrEmpty(redirectUri) || !Url.IsLocalUrl(redirectUri))
            {
                redirectUri = "/";
            }

            await HttpContext.ChallengeAsync(
                MicrosoftAccountDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = redirectUri });
        }

        [HttpGet("signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }
    }
}
