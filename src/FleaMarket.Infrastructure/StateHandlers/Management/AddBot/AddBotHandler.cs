using FleaMarket.Data;
using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Services;
using FleaMarket.Infrastructure.Services.LocalizedText;
using FleaMarket.Infrastructure.Services.MessageSender;
using FleaMarket.Infrastructure.Services.TelegramUserStateService;
using FleaMarket.Infrastructure.StateHandlers.Management.AddBotConfirmation;
using FleaMarket.Infrastructure.StateHandlers.Management.MainMenu;
using FleaMarket.Infrastructure.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FleaMarket.Infrastructure.StateHandlers.Management.AddBot;

public class AddBotHandler : BaseManagementStringStateHandler<AddBotState>
{
    private readonly IWebhookService _webhookService;

    public AddBotHandler(ILocalizedTextService localizedTextService, IMessageCommandPublisher messageCommandPublisher,
        ITelegramUserStateService telegramUserStateService, FleaMarketDatabaseContext databaseContext,
        IOptions<ApplicationConfiguration> applicationConfiguration, IServiceProvider services,
        IWebhookService webhookService) : base(
        localizedTextService, messageCommandPublisher, telegramUserStateService, databaseContext,
        applicationConfiguration, services)
    {
        _webhookService = webhookService;
    }

    protected override async Task ExecuteHandle(Guid telegramUserId, Guid? telegramBotId, AddBotState state,
        string parameter)
    {
        var cancelText = await LocalizedTextService.GetText(LocalizedTextId.Cancel, Language);

        if (parameter == cancelText)
        {
            await MainMenuHandler.Activate(telegramUserId, telegramBotId, MainMenuState.Default);
            return;
        }

        var botAlreadyExists = await BotAlreadyExists(parameter);

        if (botAlreadyExists)
        {
            var botAlreadyExistsText = await LocalizedTextService.GetText(LocalizedTextId.BotAlreadyExists, Language);
            await MessageCommandPublisher.SendTextMessage(Token, ChatId, botAlreadyExistsText);
            await Activate(telegramUserId, telegramBotId, state);
            return;
        }

        var tokenIsValid = await TokenIsValid(parameter);

        if (!tokenIsValid)
        {
            var invalidTokenText = await LocalizedTextService.GetText(LocalizedTextId.InvalidToken, Language);
            await MessageCommandPublisher.SendTextMessage(Token, ChatId, invalidTokenText);
            await Activate(telegramUserId, telegramBotId, state);
            return;
        }

        var confirmationState = new AddBotConfirmationState
        {
            Token = parameter
        };

        await AddBotConfirmationHandler.Activate(telegramUserId, telegramBotId, confirmationState);
    }

    protected override async Task ExecuteActivate(Guid telegramUserId, Guid? telegramBotId, AddBotState state)
    {
        var sendTokenText = await LocalizedTextService.GetText(LocalizedTextId.SendToken, Language);
        var cancelText = await LocalizedTextService.GetText(LocalizedTextId.Cancel, Language);
        var buttons = TelegramMenuBuilder
            .Create()
            .AddRow()
            .AddButton(cancelText)
            .Build();

        await MessageCommandPublisher.SendKeyboard(Token, ChatId, sendTokenText, buttons);
    }

    private async Task<bool> BotAlreadyExists(string token)
    {
        var tokenExists = await DatabaseContext.TelegramBots
            .AnyAsync(x => x.Token == token);

        return tokenExists;
    }

    private async Task<bool> TokenIsValid(string token)
    {
        try
        {
            await _webhookService.SetWebhook(token);
            await _webhookService.DeleteWebhook(token);
            return true;
        }
        catch
        {
            return false;
        }
    }
}