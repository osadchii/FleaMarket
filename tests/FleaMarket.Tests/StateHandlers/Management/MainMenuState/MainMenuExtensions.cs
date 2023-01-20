using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Extensions;
using FleaMarket.Infrastructure.Services.MessageSender.Models;
using FleaMarket.Tests.Constants;
using MassTransit.Testing;
using Shouldly;

namespace FleaMarket.Tests.StateHandlers.Management.MainMenuState;

public static class MainMenuExtensions
{
    public static async Task ValidateMainMenuStateActivate(this TestContext testContext, long chatId)
    {
        var mainMenuText = await testContext.GetLocalizedText(LocalizedTextId.MainMenu, Language.Russian);
        var addBotText = await testContext.GetLocalizedText(LocalizedTextId.AddBotButton, Language.Russian);
        var myBotsText = await testContext.GetLocalizedText(LocalizedTextId.MyBotsButton, Language.Russian);
        var changeLanguageText = await testContext.GetLocalizedText(LocalizedTextId.ChangeLanguageButton, Language.Russian);
        
        var published = await testContext.Harness.Published.Any<MessageCommand>(message =>
            message.MessageObject is MessageCommand messageObject &&
            messageObject.Items.Any(x => x.ChatId == chatId));
        published.ShouldBeTrue();

        var message = await testContext.Harness.Published.SelectAsync<MessageCommand>(message =>
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