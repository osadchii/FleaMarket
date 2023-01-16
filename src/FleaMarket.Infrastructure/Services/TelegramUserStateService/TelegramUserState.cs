namespace FleaMarket.Infrastructure.Services.TelegramUserStateService;

public class TelegramUserState
{
    public string Name { get; set; }
    public string StateData { get; set; }

    public TelegramUserState()
    {
        
    }

    public TelegramUserState(string name, string stateData)
    {
        Name = name;
        StateData = stateData;
    }
}