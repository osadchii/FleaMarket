using FleaMarket.Data;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Services.TelegramUserStateService;
using FleaMarket.Infrastructure.StateHandlers;
using FleaMarket.Infrastructure.StateHandlers.Management.MainMenu;
using FleaMarket.Infrastructure.StateHandlers.Management.Start;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace FleaMarket.Infrastructure.Services.UpdateHandlers;

public interface ITextMessageHandler
{
    Task Handle(string token, Message message);
}

public class TextMessageHandler : ITextMessageHandler
{
    private readonly ILogger<TextMessageHandler> _logger;
    private readonly ITelegramUserService _telegramUserService;
    private readonly ITelegramUserStateService _telegramUserStateService;
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly FleaMarketDatabaseContext _databaseContext;
    private readonly IServiceProvider _serviceProvider;

    public TextMessageHandler(ILogger<TextMessageHandler> logger, ITelegramUserService telegramUserService,
        ITelegramUserStateService telegramUserStateService, IOptions<ApplicationConfiguration> applicationConfiguration,
        FleaMarketDatabaseContext databaseContext, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _telegramUserService = telegramUserService;
        _telegramUserStateService = telegramUserStateService;
        _databaseContext = databaseContext;
        _serviceProvider = serviceProvider;
        _applicationConfiguration = applicationConfiguration.Value;
    }

    public async Task Handle(string token, Message message)
    {
        var text = message.Text;
        var chatId = message.Chat.Id;

        _logger.LogInformation("Received message with text '{Text}' from Chat with Id {Id}", text, chatId);

        var managementUpdate = token == _applicationConfiguration.ManagementBot.Token;

        Guid? botId = null;

        if (!managementUpdate)
        {
            botId = await _databaseContext.TelegramBots
                .Where(x => x.Token == token)
                .Where(x => x.IsActive)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (botId == Guid.Empty)
            {
                _logger.LogWarning("Telegram bot with Token '{Token}' not found", token);
                return;
            }
        }

        var user = await _telegramUserService.GetOrCreateTelegramUser(chatId);
        var userState = await _telegramUserStateService.GetState(user.Id, botId);

        if (userState is null)
        {
            if (managementUpdate)
            {
                var startState = _serviceProvider.GetRequiredService<IStateHandler<StartState, string>>();
                await startState.Activate(user.Id, null, new StartState());
                return;
            }
        }

        if (managementUpdate)
        {
            await RouteManagementUpdate(user.Id, userState, text);
        }
    }

    private Task RouteManagementUpdate(Guid userId, TelegramUserState telegramUserState, string text)
    {
        switch (telegramUserState.Name)
        {
            case nameof(StartState):
                var startStateHandler = _serviceProvider.GetRequiredService<IStateHandler<StartState, string>>();
                var startState = startStateHandler.DeserializeStateDate(telegramUserState.StateData);
                return startStateHandler.Handle(userId, null, startState, text);
            case nameof(MainMenuState):
                var mainMenuHandler = _serviceProvider.GetRequiredService<IStateHandler<MainMenuState, string>>();
                var mainMenuState = mainMenuHandler.DeserializeStateDate(telegramUserState.StateData);
                return mainMenuHandler.Handle(userId, null, mainMenuState, text);
            default:
                break;
        }

        return Task.CompletedTask;
    }
}