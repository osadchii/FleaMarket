using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Extensions;
using FleaMarket.Infrastructure.Services.MessageSender.Models;
using FleaMarket.Tests.Constants;
using MassTransit.Testing;
using Shouldly;

namespace FleaMarket.Tests.StateHandlers.Management.StartState;

public static class StartStateExtensions
{
    public static async Task ValidateStartStateActivate(this TestContext testContext, ITestHarness harness, long chatId)
    {
        var text = await testContext.GetLocalizedText(LocalizedTextId.SelectLanguage, Language.Russian);
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

        keyboardContent.Text.ShouldBe(text);

        var buttons = keyboardContent.Buttons
            .SelectMany(row => row.Select(x => x))
            .ToArray();

        buttons.ShouldContain(x => x == Language.English.ToString());
        buttons.ShouldContain(x => x == Language.Russian.ToString());
    }
}