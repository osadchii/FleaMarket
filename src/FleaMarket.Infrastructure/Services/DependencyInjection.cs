using FleaMarket.Infrastructure.Services.LocalizedText;
using FleaMarket.Infrastructure.Services.MessageSender;
using FleaMarket.Infrastructure.Services.TelegramUserStateService;
using FleaMarket.Infrastructure.Services.UpdateHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Infrastructure.Services;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUpdateHandleService, UpdateHandleService>();
        services.AddTransient<ITextMessageHandler, TextMessageHandler>();
        services.AddTransient<IMyChatMemberHandler, MyChatMemberHandler>();
        services.AddTransient<IWebhookService, WebhookService>();

        services.AddTransient<IMessageCommandHandler, MessageCommandHandler>();
        services.AddTransient<IMessageCommandPublisher, MessageCommandPublisher>();

        services.AddTransient<ILocalizedTextService, LocalizedTextService>();
        services.AddTransient<ITelegramUserService, TelegramUserService>();
        services.AddTransient<ITelegramUserStateService, TelegramUserStateService.TelegramUserStateService>();
    }
}