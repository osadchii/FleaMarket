namespace FleaMarket.Infrastructure.Services.MessageSender.Models;

public class TextMessageContent : MessageCommandItemContentWithChatId
{
    public string Text { get; set; }

    public TextMessageContent()
    {
        
    }

    public TextMessageContent(long chatId, string text)
    {
        ChatId = chatId;
        Text = text;
    }
}