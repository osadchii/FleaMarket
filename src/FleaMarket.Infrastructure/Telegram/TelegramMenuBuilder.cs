namespace FleaMarket.Infrastructure.Telegram;

public class TelegramMenuBuilder
{
    private readonly List<List<string>> _buttons = new();
    private List<string> _currentRow;
    private TelegramMenuBuilder()
    {
        
    }
    
    public static TelegramMenuBuilder Create()
    {
        return new TelegramMenuBuilder();
    }

    public TelegramMenuBuilder AddRow()
    {
        var newRow = new List<string>();
        _buttons.Add(newRow);
        _currentRow = newRow;
        return this;
    }

    public TelegramMenuBuilder AddButton(string text)
    {
        _currentRow.Add(text);
        return this;
    }

    public IEnumerable<IEnumerable<string>> Build()
    {
        return _buttons;
    }
}