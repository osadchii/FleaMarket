using FleaMarket.Api;

var builder = FleaMarketConfigurator.WebApplicationBuilder(args);
var app = FleaMarketConfigurator.FleaMarketApplication(builder);

app.Run();

namespace FleaMarket.Api
{
    public partial class Program
    {
    }
}