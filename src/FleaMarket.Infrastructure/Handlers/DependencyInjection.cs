using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Infrastructure.Handlers;

public static class DependencyInjection
{
    public static void AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);
    }
}