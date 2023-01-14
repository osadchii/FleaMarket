using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Infrastructure.Services;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUpdateHandleService, UpdateHandleService>();
        services.AddTransient<ITextMessageHandler, TextMessageHandler>();
        services.AddTransient<IWebhookService, WebhookService>();
    }
}