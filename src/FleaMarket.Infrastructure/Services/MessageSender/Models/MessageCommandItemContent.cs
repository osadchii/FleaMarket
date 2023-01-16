namespace FleaMarket.Infrastructure.Services.MessageSender.Models;

public abstract class MessageCommandItemContent
{
}

public class MessageCommandItemContentWithChatId : MessageCommandItemContent
{
    public long ChatId { get; set; }
}