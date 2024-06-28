using Microsoft.Extensions.Logging;
using RateLimitNotification.Domain.RateLimit.Interfaces;

namespace RateLimitNotification.Domain.RateLimit.Services
{
    public class RateLimitService : IRateLimitService
    {
        ILogger<RateLimitService> _logger;

        public RateLimitService(ILogger<RateLimitService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> CanNotify(string userId, string notificationType)
        {
            throw new NotImplementedException();
        }
    }
}
