using MassTransit;

namespace FleaMarket.Infrastructure.Services.MessageSender.Consumer;

public class MessageCommandConsumerDefinition : ConsumerDefinition<MessageCommandConsumer>
{
    public MessageCommandConsumerDefinition()
    {
        // override the default endpoint name
        EndpointName = "message-sender";

        // limit the number of messages consumed concurrently
        // this applies to the consumer only, not the endpoint
        ConcurrentMessageLimit = 5;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<MessageCommandConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100,200,500,800,1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}