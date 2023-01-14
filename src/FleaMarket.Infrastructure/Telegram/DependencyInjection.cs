using FleaMarket.Infrastructure.Telegram.Client;
using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Infrastructure.Telegram;

public static class DependencyInjection
{
    public static void AddFleaMarketTelegramBot(this IServiceCollection services)
    {
        services.AddTransient<IFleaMarketTelegramBotClient, FleaMarketTelegramBotClient>();
    }
}