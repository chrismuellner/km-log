using System;
using System.Threading.Tasks;
using KmLog.Server.Dal;
using KmLog.Server.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class AuthenticationLogic
    {
        private readonly ILogger<AuthenticationLogic> _logger;
        private readonly UserManager<User> _userManager;

        public AuthenticationLogic(ILogger<AuthenticationLogic> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<bool> CheckUser(string username, string password)
        {
            try
            {
                //var user = await _userManager.FindByNameAsync(username);
                //return user != null && await _userManager.CheckPasswordAsync(user, password);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking user by email");
                throw;
            }
        }
    }
}
