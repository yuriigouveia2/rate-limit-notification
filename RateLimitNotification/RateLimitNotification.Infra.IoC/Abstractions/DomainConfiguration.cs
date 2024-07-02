using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RateLimitNotification.Domain.Abstractions.Configurations;
using RateLimitNotification.Domain.Gateway.Interfaces;
using RateLimitNotification.Domain.Gateway.Services;
using RateLimitNotification.Domain.Notification.Interfaces;
using RateLimitNotification.Domain.Notification.Services;
using RateLimitNotification.Domain.RateLimit.Interfaces;
using RateLimitNotification.Domain.RateLimit.Services;
using RateLimitNotification.Domain.RateLimit.Strategies;

namespace RateLimitNotification.Infra.IoC.Abstractions
{
    internal static class DomainConfiguration
    {
        public static void OnConfigure(
            IServiceCollection services,
            IConfiguration configuration)
        {
            InjectServices(services);
            InjectConfiguration(services, configuration);
        }

        #region Inject Services

        private static void InjectServices(IServiceCollection services)
        {
            services
                .AddScoped<IRateLimitService, RateLimitService>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<IGatewayService, GatewayService>()
                .AddScoped<INotificationTypeStrategyResolver, NotificationTypeStrategyResolver>();
        }

        private static void InjectConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            var rateLimitConfiguration = configuration.GetSection("RateLimitConfiguration").Get<RateLimitConfiguration>();

            if (rateLimitConfiguration != null)
            {
                services.AddSingleton(rateLimitConfiguration);
            }
            else
            {
                throw new InvalidDataException("Configuration was not present in environment variables");
            }
        }

        #endregion
    }
}
