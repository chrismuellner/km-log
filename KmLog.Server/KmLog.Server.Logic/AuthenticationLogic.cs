using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class AuthenticationLogic
    {
        private readonly ILogger<CarLogic> _logger;

        public AuthenticationLogic(ILogger<CarLogic> logger)
        {
            _logger = logger;
        }
    }
}
