using FleaMarket.Infrastructure.StateHandlers.Management.Start;
using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Infrastructure.StateHandlers;

public static class DependencyInjection
{
    public static void AddStateHandlers(this IServiceCollection services)
    {
        services.AddTransient<IStateHandler<StartState, string>, StartStateHandler>();
    }
}