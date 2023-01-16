namespace FleaMarket.Infrastructure.Services.MessageSender.Models;

public class MessageCommandItem
{
    public MessageCommandItemType Type { get; set; }
    public string Content { get; set; }

    public MessageCommandItem()
    {
        
    }

    public MessageCommandItem(MessageCommandItemType type, string content)
    {
        Type = type;
        Content = content;
    }
}

public enum MessageCommandItemType
{
    Text,
    Keyboard
}