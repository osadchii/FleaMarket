using FleaMarket.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FleaMarket.Infrastructure.HostedServices;

public class WebHookHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public WebHookHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var webhookService = scope.ServiceProvider.GetRequiredService<IWebhookService>();
        await webhookService.SetWebhooks();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var webhookService = scope.ServiceProvider.GetRequiredService<IWebhookService>();
        await webhookService.DeleteWebhooks();
    }
}