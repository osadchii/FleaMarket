using FleaMarket.Infrastructure.StateHandlers.Management.AddBot;
using FleaMarket.Infrastructure.StateHandlers.Management.AddBotConfirmation;
using FleaMarket.Infrastructure.StateHandlers.Management.MainMenu;
using FleaMarket.Infrastructure.StateHandlers.Management.Start;
using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Infrastructure.StateHandlers;

public static class DependencyInjection
{
    public static void AddStateHandlers(this IServiceCollection services)
    {
        services.AddTransient<IStateHandler<StartState, string>, StartStateHandler>();
        services.AddTransient<IStateHandler<MainMenuState, string>, MainMenuHandler>();
        services.AddTransient<IStateHandler<AddBotConfirmationState, string>, AddBotConfirmationHandler>();
        services.AddTransient<IStateHandler<AddBotState, string>, AddBotHandler>();
    }
}