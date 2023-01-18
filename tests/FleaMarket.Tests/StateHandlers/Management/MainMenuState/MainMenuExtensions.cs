using FleaMarket.Infrastructure.Extensions;
using FleaMarket.Infrastructure.Services.MessageSender.Models;
using FleaMarket.Tests.Constants;
using MassTransit.Testing;
using Shouldly;

namespace FleaMarket.Tests.StateHandlers.Management.MainMenuState;

public static class MainMenuExtensions
{
    public static async Task ValidateMainMenuStateActivate(this ITestHarness harness, long chatId, string mainMenuText,
        string addBotText, string myBotsText, string changeLanguageText)
    {
        var published = await harness.Published.Any<MessageCommand>(message =>
            message.MessageObject is MessageCommand messageObject &&
            messageObject.Items.Any(x => x.ChatId == chatId));
        published.ShouldBeTrue();

        var message = await harness.Published.SelectAsync<MessageCommand>(message =>
            message.MessageObject is MessageCommand messageObject &&
            messageObject.Items.Any(x => x.ChatId == chatId)).First();
        message.ShouldNotBeNull();

        var messageObject = message.MessageObject as MessageCommand;
        messageObject.ShouldNotBeNull();

        messageObject.Token.ShouldBe(TestConstant.ManagementBotToken);
        messageObject.Items.Count.ShouldBe(1);

        var messageItem = messageObject.Items.First();
        messageItem.Type.ShouldBe(MessageCommandItemType.Keyboard);
        messageItem.ChatId.HasValue.ShouldBeTrue();
        messageItem.ChatId!.Value.ShouldBe(chatId);

        var keyboardContent = messageItem.Content.FromJson<KeyboardMessageContent>();
        keyboardContent.ShouldNotBeNull();

        keyboardContent.Text.ShouldBe(mainMenuText);

        var buttons = keyboardContent.Buttons
            .SelectMany(row => row.Select(x => x))
            .ToArray();

        buttons.ShouldContain(x => x == addBotText);
        buttons.ShouldContain(x => x == myBotsText);
        buttons.ShouldContain(x => x == changeLanguageText);
    }
}