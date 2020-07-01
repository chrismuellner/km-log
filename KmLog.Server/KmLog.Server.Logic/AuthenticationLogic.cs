using System;
using System.Threading.Tasks;
using KmLog.Server.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class AuthenticationLogic
    {
        private readonly ILogger<AuthenticationLogic> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationLogic(ILogger<AuthenticationLogic> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                return await _unitOfWork.UserRepository.Query()
                    .AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking user by email");
                throw;
            }
        }
    }
}
