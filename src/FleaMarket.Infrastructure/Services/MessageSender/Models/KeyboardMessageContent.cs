namespace FleaMarket.Infrastructure.Services.MessageSender.Models;

public class KeyboardMessageContent : MessageCommandItemContent
{
    public string Text { get; set; }
    public IEnumerable<IEnumerable<string>> Buttons { get; set; }

    public KeyboardMessageContent()
    {
        
    }

    public KeyboardMessageContent(string text, IEnumerable<IEnumerable<string>> buttons)
    {
        Text = text;
        Buttons = buttons;
    }
}