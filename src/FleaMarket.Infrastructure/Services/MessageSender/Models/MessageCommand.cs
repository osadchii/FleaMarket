namespace FleaMarket.Infrastructure.Services.MessageSender.Models;

public class MessageCommand
{
    public string Token { get; set; }
    public List<MessageCommandItem> Items { get; set; } = new();
}