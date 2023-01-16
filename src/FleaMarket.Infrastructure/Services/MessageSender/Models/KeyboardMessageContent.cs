namespace FleaMarket.Infrastructure.Services.MessageSender.Models;

public class KeyboardMessageContent : MessageCommandItemContentWithChatId
{
    public string Text { get; set; }
    public IEnumerable<IEnumerable<string>> Buttons { get; set; }

    public KeyboardMessageContent()
    {
        
    }

    public KeyboardMessageContent(long chatId, string text, IEnumerable<IEnumerable<string>> buttons)
    {
        ChatId = chatId;
        Text = text;
        Buttons = buttons;
    }
}