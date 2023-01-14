namespace FleaMarket.Infrastructure.Services.MessageSender.Models;

public class MessageCommandItem
{
    public MessageCommandItemType Type { get; set; }
    public string Content { get; set; }
}

public enum MessageCommandItemType
{
    Text
}