using System;
using System.Threading.Tasks;
using KmLog.Server.Dal;
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

                return await _unitOfWork.UserRepository.CheckByEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking user by email");
                throw;
            }
        }
    }
}
