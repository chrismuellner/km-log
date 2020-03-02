using System.Security.Claims;

namespace KmLog.Server.WebApi.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            var email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
            return email;
        }
    }
}
