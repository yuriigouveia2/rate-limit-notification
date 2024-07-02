using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RateLimitNotification.Infra.IoC.Abstractions;

namespace RateLimitNotification.Infra.IoC
{
    public static class IoC
    {
        public static void OnConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            DomainConfiguration.OnConfigure(services, configuration);
            InfraConfiguration.OnConfigure(services, configuration);
        }
    }
}
