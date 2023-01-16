using FleaMarket.Infrastructure.Extensions;
using FleaMarket.Infrastructure.Services.MessageSender.Models;
using FleaMarket.Infrastructure.Telegram.Client;

namespace FleaMarket.Infrastructure.Services.MessageSender;

public interface IMessageCommandHandler
{
    Task Handle(MessageCommand command);
}

public class MessageCommandHandler : IMessageCommandHandler
{
    private readonly IFleaMarketTelegramBotClient _telegramBotClient;

    public MessageCommandHandler(IFleaMarketTelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task Handle(MessageCommand command)
    {
        foreach (var item in command.Items)
        {
            switch (item.Type)
            {
                case MessageCommandItemType.Text:
                    var textContent = item.Content.FromJson<TextMessageContent>();
                    await _telegramBotClient.SendTextMessage(command.Token, textContent.ChatId, textContent.Text);
                    break;
                case MessageCommandItemType.Keyboard:
                    var keyboardContent = item.Content.FromJson<KeyboardMessageContent>();
                    await _telegramBotClient.SendKeyboard(command.Token, keyboardContent.ChatId, keyboardContent.Text,
                        keyboardContent.Buttons);
                    break;
                default:
                    continue;
            }
        }
    }
}