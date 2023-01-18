using FleaMarket.Data;
using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Extensions;
using FleaMarket.Infrastructure.Services.LocalizedText;
using FleaMarket.Infrastructure.Services.MessageSender;
using FleaMarket.Infrastructure.Services.TelegramUserStateService;
using FleaMarket.Infrastructure.StateHandlers.Management.AddBot;
using FleaMarket.Infrastructure.StateHandlers.Management.AddBotConfirmation;
using FleaMarket.Infrastructure.StateHandlers.Management.MainMenu;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FleaMarket.Infrastructure.StateHandlers.Management;

public abstract class BaseManagementStringStateHandler<TState> : IStateHandler<TState, string> where TState : BaseState
{
    protected readonly ILocalizedTextService LocalizedTextService;
    protected readonly IMessageCommandPublisher MessageCommandPublisher;
    protected readonly ITelegramUserStateService TelegramUserStateService;
    protected readonly FleaMarketDatabaseContext DatabaseContext;
    protected readonly ApplicationConfiguration ApplicationConfiguration;
    protected readonly IServiceProvider Services;

    protected IStateHandler<MainMenuState, string> MainMenuHandler =>
        Services.GetRequiredService<IStateHandler<MainMenuState, string>>();
    protected IStateHandler<AddBotState, string> AddBotHandler =>
        Services.GetRequiredService<IStateHandler<AddBotState, string>>();
    protected IStateHandler<AddBotConfirmationState, string> AddBotConfirmationHandler =>
        Services.GetRequiredService<IStateHandler<AddBotConfirmationState, string>>();

    protected long ChatId { get; private set; }
    protected string Token { get; }
    protected Language Language { get; private set; }

    protected BaseManagementStringStateHandler(ILocalizedTextService localizedTextService,
        IMessageCommandPublisher messageCommandPublisher, ITelegramUserStateService telegramUserStateService,
        FleaMarketDatabaseContext databaseContext, IOptions<ApplicationConfiguration> applicationConfiguration,
        IServiceProvider services)
    {
        LocalizedTextService = localizedTextService;
        MessageCommandPublisher = messageCommandPublisher;
        TelegramUserStateService = telegramUserStateService;
        DatabaseContext = databaseContext;
        Services = services;
        ApplicationConfiguration = applicationConfiguration.Value;

        Token = ApplicationConfiguration.ManagementBot.Token;
    }

    protected abstract Task ExecuteHandle(Guid telegramUserId, Guid? telegramBotId, TState state, string parameter);
    protected abstract Task ExecuteActivate(Guid telegramUserId, Guid? telegramBotId, TState state);

    public async Task Handle(Guid telegramUserId, Guid? telegramBotId, TState state, string parameter)
    {
        await FillUserData(telegramUserId);
        await ExecuteHandle(telegramUserId, telegramBotId, state, parameter);
    }

    public async Task Activate(Guid telegramUserId, Guid? telegramBotId, TState state)
    {
        await FillUserData(telegramUserId);

        var telegramUserData = await DatabaseContext.TelegramUsers
            .AsNoTracking()
            .Where(x => x.Id == telegramUserId)
            .Select(x => new { x.ChatId, x.Language })
            .FirstOrDefaultAsync();

        ChatId = telegramUserData.ChatId;
        Language = telegramUserData.Language ?? Language.Russian;

        await ExecuteActivate(telegramUserId, telegramBotId, state);
        var userStateName = state.GetType().Name;
        var userState = new TelegramUserState(userStateName, state.ToJson());
        await TelegramUserStateService.SetState(telegramUserId, telegramBotId, userState);
    }

    private async Task FillUserData(Guid telegramUserId)
    {
        var telegramUserData = await DatabaseContext.TelegramUsers
            .AsNoTracking()
            .Where(x => x.Id == telegramUserId)
            .Select(x => new { x.ChatId, x.Language })
            .FirstOrDefaultAsync();

        ChatId = telegramUserData.ChatId;
        Language = telegramUserData.Language ?? Language.Russian;
    }
}