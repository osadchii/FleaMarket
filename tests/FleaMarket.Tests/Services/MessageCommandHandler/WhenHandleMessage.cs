using FleaMarket.Infrastructure.Services.MessageSender;
using FleaMarket.Infrastructure.Services.MessageSender.Models;
using FleaMarket.Infrastructure.Telegram;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace FleaMarket.Tests.Services.MessageCommandHandler;

public class WhenHandleMessage : TestContext
{
    private readonly IMessageCommandHandler _messageCommandHandler;

    public WhenHandleMessage(TestWebApplicationFactory factory) : base(factory)
    {
        _messageCommandHandler = Services.GetRequiredService<IMessageCommandHandler>();
    }

    [Fact]
    public async Task ShouldSendTextMessage()
    {
        // Arrange

        var token = UniqueText;
        var chatId = new Random(DateTime.Now.Millisecond).NextInt64();
        var text = UniqueText;

        var command = new MessageCommand(token);
        var content = new TextMessageContent(chatId, text);
        command.AddItem(MessageCommandItemType.Text, content);

        // Act

        await _messageCommandHandler.Handle(command);

        // Assert

        var messages = TelegramBotClient.TextMessages;
        messages.ShouldContain(x =>
            x.Token == token &&
            x.ChatId == chatId &&
            x.Text == text);
    }

    [Fact]
    public async Task ShouldSendKeyboardMessage()
    {
        // Arrange

        var token = UniqueText;
        var chatId = new Random(DateTime.Now.Millisecond).NextInt64();
        var text = UniqueText;

        var command = new MessageCommand(token);

        var buttons = TelegramMenuBuilder.Create()
            .AddRow()
            .AddButton(UniqueText)
            .AddButton(UniqueText)
            .AddRow()
            .AddButton(UniqueText)
            .Build()
            .ToArray();

        var allButtons = buttons
            .SelectMany(x => x
                .Select(y => y))
            .ToArray();

        var content = new KeyboardMessageContent(chatId, text, buttons);
        command.AddItem(MessageCommandItemType.Keyboard, content);

        // Act

        await _messageCommandHandler.Handle(command);

        // Assert

        var messages = TelegramBotClient.KeyboardMessages;
        messages.ShouldContain(x =>
            x.Token == token &&
            x.ChatId == chatId &&
            x.Text == text &&
            x.Buttons
                .SelectMany(row => row.Select(button => button))
                .Intersect(allButtons)
                .Count() == allButtons.Count());
    }

    [Fact]
    public async Task ShouldSendMultipleTextMessages()
    {
        // Arrange

        var random = new Random(DateTime.Now.Millisecond);

        var token = UniqueText;

        var chatId1 = random.NextInt64();
        var text1 = UniqueText;

        var chatId2 = random.NextInt64();
        var text2 = UniqueText;

        var command = new MessageCommand(token);

        var content1 = new TextMessageContent(chatId1, text1);
        command.AddItem(MessageCommandItemType.Text, content1);

        var content2 = new TextMessageContent(chatId2, text2);
        command.AddItem(MessageCommandItemType.Text, content2);

        // Act

        await _messageCommandHandler.Handle(command);

        // Assert

        var messages = TelegramBotClient.TextMessages;
        messages.ShouldContain(x =>
            x.Token == token &&
            x.ChatId == chatId1 &&
            x.Text == text1);

        messages.ShouldContain(x =>
            x.Token == token &&
            x.ChatId == chatId2 &&
            x.Text == text2);
    }
}