using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KmLog.Server.Domain;
using KmLog.Server.Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace KmLog.Server.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly AuthenticationLogic _authenticationLogic;

        public AuthenticationController(ILogger<AuthenticationController> logger, AuthenticationLogic authenticationLogic)
        {
            _logger = logger;
            _authenticationLogic = authenticationLogic;
        }

        [HttpGet("login")]
        public async Task<IActionResult> GetUser()
        {
            var email = "";
            var password = "";
            if (email != null && await _authenticationLogic.CheckUser(email, password))
            {
                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RANDOM_STRING123"));
                var token = new JwtSecurityToken(
                    issuer: "https://kmlog.dev",
                    audience: "https://kmlog.dev",
                    expires: DateTime.Now.AddDays(7),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            } 

            _logger.LogWarning($"Unregistered user with email '{email}' tried to login!");
            return Unauthorized();
        }
    }
}
