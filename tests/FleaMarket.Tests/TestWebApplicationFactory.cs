using FleaMarket.Api;
using FleaMarket.Data;
using FleaMarket.Data.Entities;
using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Telegram.Client;
using FleaMarket.Tests.Constants;
using MassTransit;
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

            var dbName = Guid.NewGuid().ToString();

            services.AddDbContext<FleaMarketDatabaseContext>(options =>
            {
                options.UseInMemoryDatabase(dbName);
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
            EnsureLocalizedTexts(db);

            services.AddSingleton<IFleaMarketTelegramBotClient, TestTelegramBotClient>();
            services.AddMassTransitTestHarness();
        });
        base.ConfigureWebHost(builder);
    }

    private static void RemoveService(IServiceCollection services, Type type)
    {
        var descriptor =
            services.SingleOrDefault(d => d.ServiceType == type)!;
        services.Remove(descriptor);
    }

    private static void EnsureLocalizedTexts(FleaMarketDatabaseContext databaseContext)
    {
        foreach (var textId in Enum.GetValues<LocalizedTextId>())
        {
            var text = Guid.NewGuid().ToString();
            foreach (var language in Enum.GetValues<Language>())
            {
                var exists = databaseContext.LocalizedTexts
                    .Where(x => x.Language == language)
                    .Any(x => x.LocalizedTextId == textId);

                if (exists)
                {
                    continue;
                }
                var entity = new LocalizedTextEntity
                {
                    LocalizedText = text,
                    Language = language,
                    LocalizedTextId = textId
                };

                databaseContext.LocalizedTexts.Add(entity);
                databaseContext.SaveChanges();
            }
        }
    }
}