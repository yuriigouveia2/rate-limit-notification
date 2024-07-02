using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RateLimitNotification.Domain.Gateway.Interfaces;
using RateLimitNotification.Domain.Gateway.Services;
using RateLimitNotification.Domain.Notification.Interfaces;
using RateLimitNotification.Domain.Notification.Services;
using RateLimitNotification.Infra.RateLimit;
using StackExchange.Redis;

namespace RateLimitNotification.Infra.IoC.Abstractions
{
    internal static class InfraConfiguration
    {
        public static void OnConfigure(
            IServiceCollection services,
            IConfiguration configuration)
        {
            InjectRepositories(services);

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
        }

        #region Inject Repositories

        private static void InjectRepositories(IServiceCollection services)
        {
            services
                .AddScoped<IRateLimitCacheRepository, RateLimitCacheRepository>();
        }

        #endregion
    }
}
