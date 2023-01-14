namespace FleaMarket.Infrastructure.Configurations;

public class ApplicationConfiguration
{
    public string Host { get; set; }
    public ManagementBotConfiguration ManagementBot { get; set; }
}

public class ManagementBotConfiguration
{
    public string Token { get; set; }
}