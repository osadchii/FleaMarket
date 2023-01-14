using FleaMarket.Api;
using FleaMarket.Data;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Telegram.Client;
using FleaMarket.Tests.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Environment = FleaMarket.Infrastructure.Constants.Environment;

namespace FleaMarket.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environment.Test);
        builder.ConfigureTestServices(services =>
        {
            RemoveService(services, typeof(DbContextOptions<FleaMarketDatabaseContext>));
            RemoveService(services, typeof(IFleaMarketTelegramBotClient));

            services.AddDbContext<FleaMarketDatabaseContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            services.Configure<ApplicationConfiguration>(options =>
            {
                options.Host = TestConstant.Host;
                options.ManagementBot = new ManagementBotConfiguration
                {
                    Token = TestConstant.ManagementBotToken
                };
            });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<FleaMarketDatabaseContext>();

            db.Database.EnsureCreated();

            services.AddSingleton<IFleaMarketTelegramBotClient, TestTelegramBotClient>();
        });
        base.ConfigureWebHost(builder);
    }

    private static void RemoveService(IServiceCollection services, Type type)
    {
        var descriptor =
            services.SingleOrDefault(d => d.ServiceType == type)!;
        services.Remove(descriptor);
    }
}