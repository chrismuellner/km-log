using System;
using System.Threading.Tasks;
using KmLog.Server.Dal;
using KmLog.Server.Dto;
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

        public async Task CreateUserIfNew(string email)
        {
            try
            {
                var user = await _userRepository.LoadByEmail(email);

                if (user == null)
                {
                    await _userRepository.Add(new UserDto
                    {
                        Email = email
                    });
                    _logger.LogInformation($"Created new user with email '{email}'");
                }
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new user");
                throw;
            }
        }
    }
}
