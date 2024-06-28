using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RateLimitNotification.Domain.Gateway.Interfaces;
using RateLimitNotification.Domain.Gateway.Services;
using RateLimitNotification.Domain.Notification.Interfaces;
using RateLimitNotification.Domain.Notification.Services;
using RateLimitNotification.Domain.RateLimit.Interfaces;
using RateLimitNotification.Domain.RateLimit.Services;

namespace RateLimitNotification.Infra.IoC.Abstractions
{
    internal static class DomainConfiguration
    {
        public static void OnConfigure(
            IServiceCollection services,
            IConfiguration configuration)
        {
            InjectServices(services);
        }

        #region Inject Services

        private static void InjectServices(IServiceCollection services)
        {
            services
                .AddScoped<IRateLimitService, RateLimitService>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<IGatewayService, GatewayService>();
        }

        #endregion
    }
}
