using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Services.MessageSender.Consumer;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Infrastructure.Services.MessageSender;

public static class DependencyInjection
{
    public static void AddSender(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfiguration = configuration.GetSection("RabbitMQ").Get<RabbitMqConfiguration>()!;
        services.AddMassTransit(x =>
        {
            x.AddConsumer<MessageCommandConsumer>(typeof(MessageCommandConsumerDefinition));
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConfiguration.Host, "/", h =>
                {
                    h.Username(rabbitMqConfiguration.User);
                    h.Password(rabbitMqConfiguration.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}