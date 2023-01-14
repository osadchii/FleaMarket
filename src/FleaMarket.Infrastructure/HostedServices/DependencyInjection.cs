using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Infrastructure.HostedServices;

public static class DependencyInjection
{
    public static void AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<WebHookHostedService>();
    }
}