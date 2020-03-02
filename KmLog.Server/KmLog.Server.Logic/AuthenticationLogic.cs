using System;
using System.Threading.Tasks;
using KmLog.Server.Dal;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class AuthenticationLogic
    {
        private readonly ILogger<AuthenticationLogic> _logger;
        private readonly IUserRepository _userRepository;

        public AuthenticationLogic(ILogger<AuthenticationLogic> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            try
            {
                return await _userRepository.CheckByEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking user by email");
                throw;
            }
        }
    }
}
