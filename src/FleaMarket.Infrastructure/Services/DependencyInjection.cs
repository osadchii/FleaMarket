using FleaMarket.Infrastructure.Services.MessageSender;
using FleaMarket.Infrastructure.Services.UpdateHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Infrastructure.Services;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUpdateHandleService, UpdateHandleService>();
        services.AddTransient<ITextMessageHandler, TextMessageHandler>();
        services.AddTransient<IWebhookService, WebhookService>();

        services.AddTransient<IMessageCommandHandler, MessageCommandHandler>();
        services.AddTransient<IMessageCommandPublisher, MessageCommandPublisher>();
    }
}