using FleaMarket.Data;
using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Extensions;
using FleaMarket.Infrastructure.Services.LocalizedText;
using FleaMarket.Infrastructure.Services.MessageSender;
using FleaMarket.Infrastructure.Services.TelegramUserStateService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FleaMarket.Infrastructure.StateHandlers.Management.Start;

public class StartStateHandler : IStateHandler<StartState, string>
{
    private readonly ILocalizedTextService _localizedTextService;
    private readonly IMessageCommandPublisher _messageCommandPublisher;
    private readonly ITelegramUserStateService _telegramUserStateService;
    private readonly FleaMarketDatabaseContext _databaseContext;
    private readonly ApplicationConfiguration _applicationConfiguration;


    public StartStateHandler(ILocalizedTextService localizedTextService,
        IMessageCommandPublisher messageCommandPublisher, ITelegramUserStateService telegramUserStateService,
        FleaMarketDatabaseContext databaseContext, IOptions<ApplicationConfiguration> applicationConfiguration)
    {
        _localizedTextService = localizedTextService;
        _messageCommandPublisher = messageCommandPublisher;
        _telegramUserStateService = telegramUserStateService;
        _databaseContext = databaseContext;
        _applicationConfiguration = applicationConfiguration.Value;
    }

    public async Task Handle(Guid telegramUserId, Guid? telegramBotId, StartState state, string parameter)
    {
        var chatId = await _databaseContext.TelegramUsers
            .Where(x => x.Id == telegramUserId)
            .Select(x => x.ChatId)
            .FirstOrDefaultAsync();
        await _messageCommandPublisher.SendTextMessage(_applicationConfiguration.ManagementBot.Token, chatId, parameter);
    }

    public async Task Activate(Guid telegramUserId, Guid? telegramBotId, StartState state)
    {
        var chatId = await _databaseContext.TelegramUsers
            .Where(x => x.Id == telegramUserId)
            .Select(x => x.ChatId)
            .FirstOrDefaultAsync();
        
        var userState = new TelegramUserState(nameof(StartState), new StartState().ToJson());
        
        var text = await _localizedTextService.GetText(LocalizedTextId.SelectLanguage, Language.English);
        
        var rows = new List<List<string>>();
        var row = new List<string>
        {
            Language.English.ToString(),
            Language.Russian.ToString()
        };
        rows.Add(row);

        await _messageCommandPublisher.SendKeyboard(_applicationConfiguration.ManagementBot.Token, chatId, text, rows);
        await _telegramUserStateService.SetState(telegramUserId, null, userState);
    }
}