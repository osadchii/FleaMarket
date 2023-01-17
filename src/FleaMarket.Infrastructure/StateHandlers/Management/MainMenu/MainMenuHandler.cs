using FleaMarket.Data;
using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Services.LocalizedText;
using FleaMarket.Infrastructure.Services.MessageSender;
using FleaMarket.Infrastructure.Services.TelegramUserStateService;
using FleaMarket.Infrastructure.Telegram;
using Microsoft.Extensions.Options;

namespace FleaMarket.Infrastructure.StateHandlers.Management.MainMenu;

public class MainMenuHandler : BaseManagementStringStateHandler<MainMenuState>
{
    public MainMenuHandler(ILocalizedTextService localizedTextService, IMessageCommandPublisher messageCommandPublisher,
        ITelegramUserStateService telegramUserStateService, FleaMarketDatabaseContext databaseContext,
        IOptions<ApplicationConfiguration> applicationConfiguration) : base(localizedTextService,
        messageCommandPublisher, telegramUserStateService, databaseContext, applicationConfiguration)
    {
    }

    protected override Task ExecuteHandle(Guid telegramUserId, Guid? telegramBotId, MainMenuState state,
        string parameter)
    {
        return Task.CompletedTask;
    }

    protected override async Task ExecuteActivate(Guid telegramUserId, Guid? telegramBotId, MainMenuState state)
    {
        var mainMenuText = await LocalizedTextService.GetText(LocalizedTextId.MainMenu, Language);
        var addBotText = await LocalizedTextService.GetText(LocalizedTextId.AddBotButton, Language);
        var myBotsText = await LocalizedTextService.GetText(LocalizedTextId.MyBotsButton, Language);
        var changeLanguage = await LocalizedTextService.GetText(LocalizedTextId.ChangeLanguageButton, Language);

        var buttons = TelegramMenuBuilder
            .Create()
            .AddRow()
            .AddButton(addBotText)
            .AddButton(myBotsText)
            .AddRow()
            .AddButton(changeLanguage)
            .Build();

        await MessageCommandPublisher.SendKeyboard(Token, ChatId, mainMenuText, buttons);
    }
}