namespace FleaMarket.Infrastructure.Services.LocalizedText.Exceptions;

public class LocalizedTextNotFoundException : Exception
{
    public LocalizedTextNotFoundException() : base("Localized text not found")
    {
        
    }
}