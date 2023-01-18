namespace FleaMarket.Infrastructure.Services.MessageSender.Models;

public class TextMessageContent : MessageCommandItemContent
{
    public string Text { get; set; }

    public TextMessageContent()
    {
        
    }

    public TextMessageContent(string text)
    {
        Text = text;
    }
}