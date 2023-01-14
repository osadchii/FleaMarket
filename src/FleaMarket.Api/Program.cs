using FleaMarket.Api;

var builder = FleaMarketConfigurator.WebApplicationBuilder(args);
var app = FleaMarketConfigurator.FleaMarketApplication(builder);

app.Run();

public partial class Program
{
}