using FleaMarket.Infrastructure.Extensions;

namespace FleaMarket.Infrastructure.Services.MessageSender.Models;

public class MessageCommand
{
    public string Token { get; set; }
    public List<MessageCommandItem> Items { get; set; } = new();

    public MessageCommand()
    {
        
    }

    public MessageCommand(string token)
    {
        Token = token;
    }

    public void AddItem(MessageCommandItemType type, long? chatId, MessageCommandItemContent content)
    {
        var textContent = content.ToJson();
        var item = new MessageCommandItem(type, chatId, textContent);
        
        Items.Add(item);
    }
}