using FleaMarket.Data;
using FleaMarket.Data.Entities;
using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Services;
using FleaMarket.Infrastructure.Services.LocalizedText;
using FleaMarket.Infrastructure.Services.MessageSender;
using FleaMarket.Infrastructure.Services.TelegramUserStateService;
using FleaMarket.Infrastructure.StateHandlers.Management.MainMenu;
using FleaMarket.Infrastructure.Telegram;
using Microsoft.Extensions.Options;

namespace FleaMarket.Infrastructure.StateHandlers.Management.AddBotConfirmation;

public class AddBotConfirmationHandler : BaseManagementStringStateHandler<AddBotConfirmationState>
{
    private readonly IWebhookService _webhookService;

    public AddBotConfirmationHandler(ILocalizedTextService localizedTextService,
        IMessageCommandPublisher messageCommandPublisher, ITelegramUserStateService telegramUserStateService,
        FleaMarketDatabaseContext databaseContext, IOptions<ApplicationConfiguration> applicationConfiguration,
        IServiceProvider services, IWebhookService webhookService) : base(localizedTextService, messageCommandPublisher,
        telegramUserStateService, databaseContext, applicationConfiguration, services)
    {
        _webhookService = webhookService;
    }

    protected override async Task ExecuteHandle(Guid telegramUserId, Guid? telegramBotId, AddBotConfirmationState state,
        string parameter)
    {
        var cancelText = await LocalizedTextService.GetText(LocalizedTextId.Cancel, Language);
        var addText = await LocalizedTextService.GetText(LocalizedTextId.Add, Language);

        if (parameter == cancelText)
        {
            await MainMenuHandler.Activate(telegramUserId, telegramBotId, new MainMenuState());
            return;
        }

        if (parameter == addText)
        {
            await AddBot(telegramUserId, state.Token);

            await MessageCommandPublisher.SendTextMessage(Token, ChatId, LocalizedTextId.AddedBot, Language);
            return;
        }

        await Activate(telegramUserId, telegramBotId, state);
    }

    protected override async Task ExecuteActivate(Guid telegramUserId, Guid? telegramBotId,
        AddBotConfirmationState state)
    {
        var addText = await LocalizedTextService.GetText(LocalizedTextId.Add, Language);
        var cancelText = await LocalizedTextService.GetText(LocalizedTextId.Cancel, Language);

        var buttons = TelegramMenuBuilder
            .Create()
            .AddRow()
            .AddButton(addText)
            .AddButton(cancelText)
            .Build();

        await MessageCommandPublisher.SendKeyboard(Token, ChatId, LocalizedTextId.AddBotTokenConfirmation, Language, buttons);
    }

    private async Task AddBot(Guid telegramUserId, string token)
    {
        var bot = new TelegramBotEntity
        {
            Token = token,
            OwnerId = telegramUserId,
            IsActive = true
        };

        await DatabaseContext.AddAsync(bot);
        await DatabaseContext.SaveChangesAsync();
        await _webhookService.SetWebhook(token);

        await MainMenuHandler.Activate(telegramUserId, null, MainMenuState.Default);
    }
}