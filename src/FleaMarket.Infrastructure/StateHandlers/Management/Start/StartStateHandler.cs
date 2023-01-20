using FleaMarket.Data;
using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Services.LocalizedText;
using FleaMarket.Infrastructure.Services.MessageSender;
using FleaMarket.Infrastructure.Services.TelegramUserStateService;
using FleaMarket.Infrastructure.StateHandlers.Management.MainMenu;
using FleaMarket.Infrastructure.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FleaMarket.Infrastructure.StateHandlers.Management.Start;

public class StartStateHandler : BaseManagementStringStateHandler<StartState>
{
    public StartStateHandler(ILocalizedTextService localizedTextService,
        IMessageCommandPublisher messageCommandPublisher, ITelegramUserStateService telegramUserStateService,
        FleaMarketDatabaseContext databaseContext, IOptions<ApplicationConfiguration> applicationConfiguration,
        IServiceProvider services) : base(localizedTextService, messageCommandPublisher, telegramUserStateService,
        databaseContext, applicationConfiguration, services)
    {
    }

    protected override async Task ExecuteHandle(Guid telegramUserId, Guid? telegramBotId, StartState state,
        string parameter)
    {
        if (Enum.TryParse<Language>(parameter, out var language))
        {
            var telegramUser = await DatabaseContext.TelegramUsers
                .FirstOrDefaultAsync(x => x.Id == telegramUserId);

            if (telegramUser is null)
            {
                return;
            }

            telegramUser.Language = language;
            await DatabaseContext.SaveChangesAsync();
            await MainMenuHandler.Activate(telegramUserId, telegramBotId, new MainMenuState());
            return;
        }

        await Activate(telegramUserId, telegramBotId, state);
    }

    protected override async Task ExecuteActivate(Guid telegramUserId, Guid? telegramBotId, StartState state)
    {
        var buttons = TelegramMenuBuilder
            .Create()
            .AddRow()
            .AddButton(Language.English.ToString())
            .AddButton(Language.Russian.ToString())
            .Build();

        await MessageCommandPublisher.SendKeyboard(Token, ChatId, LocalizedTextId.SelectLanguage, Language.English, buttons);
    }
}