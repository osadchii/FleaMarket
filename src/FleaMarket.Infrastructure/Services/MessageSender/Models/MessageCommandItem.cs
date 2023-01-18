using Telegram.Bot.Types;

namespace FleaMarket.Infrastructure.Services.MessageSender.Models;

public class MessageCommandItem
{
    public MessageCommandItemType Type { get; set; }
    public long? ChatId { get; set; }
    public string Content { get; set; }

    public MessageCommandItem()
    {
        
    }

    public MessageCommandItem(MessageCommandItemType type, long? chatId, string content)
    {
        Type = type;
        ChatId = chatId;
        Content = content;
    }
}

public enum MessageCommandItemType
{
    Text,
    Keyboard
}