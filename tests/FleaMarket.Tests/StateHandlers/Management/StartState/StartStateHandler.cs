using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.StateHandlers;
using FleaMarket.Tests.StateHandlers.Management.MainMenuState;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace FleaMarket.Tests.StateHandlers.Management.StartState;

public class StartStateHandler : TestContext
{
    private readonly IStateHandler<Infrastructure.StateHandlers.Management.Start.StartState, string> _startStateHandler;

    public StartStateHandler(TestWebApplicationFactory factory) : base(factory)
    {
        _startStateHandler =
            Services
                .GetRequiredService<IStateHandler<Infrastructure.StateHandlers.Management.Start.StartState, string>>();
    }

    [Fact]
    public async Task ShouldBeActivated()
    {
        // Arrange

        var user = await CreateTelegramUser();
        var text = await DatabaseContext.LocalizedTexts
            .Where(x => x.Language == Language.Russian)
            .Where(x => x.LocalizedTextId == LocalizedTextId.SelectLanguage)
            .Select(x => x.LocalizedText)
            .FirstAsync();

        await Harness.Start();

        // Act

        await _startStateHandler.Activate(user.Id, null,
            new Infrastructure.StateHandlers.Management.Start.StartState());

        // Assert

        await Harness.ValidateStartStateActivate(text, user.ChatId);
    }

    [Fact]
    public async Task ShouldSetLanguage()
    {
        // Arrange

        var user = await CreateTelegramUser();
        var mainMenuText = await DatabaseContext.LocalizedTexts
            .Where(x => x.Language == Language.Russian)
            .Where(x => x.LocalizedTextId == LocalizedTextId.SelectLanguage)
            .Select(x => x.LocalizedText)
            .FirstAsync();
        var addBotText = await DatabaseContext.LocalizedTexts
            .Where(x => x.Language == Language.Russian)
            .Where(x => x.LocalizedTextId == LocalizedTextId.SelectLanguage)
            .Select(x => x.LocalizedText)
            .FirstAsync();
        var myBotsText = await DatabaseContext.LocalizedTexts
            .Where(x => x.Language == Language.Russian)
            .Where(x => x.LocalizedTextId == LocalizedTextId.SelectLanguage)
            .Select(x => x.LocalizedText)
            .FirstAsync();
        var changeLanguageText = await DatabaseContext.LocalizedTexts
            .Where(x => x.Language == Language.Russian)
            .Where(x => x.LocalizedTextId == LocalizedTextId.SelectLanguage)
            .Select(x => x.LocalizedText)
            .FirstAsync();

        await Harness.Start();

        // Act

        await _startStateHandler.Handle(user.Id, null, new Infrastructure.StateHandlers.Management.Start.StartState(),
            Language.Russian.ToString());

        // Assert

        await Harness.ValidateMainMenuStateActivate(user.ChatId, mainMenuText,
            addBotText, myBotsText,
            changeLanguageText);

        var userLanguage = await DatabaseContext.TelegramUsers
            .AsNoTracking()
            .Where(x => x.Id == user.Id)
            .Select(x => x.Language)
            .FirstOrDefaultAsync();

        userLanguage.ShouldNotBeNull();
        userLanguage.Value.ShouldBe(Language.Russian);
    }

    [Fact]
    public async Task ShouldBeReactivated()
    {
        // Arrange

        var user = await CreateTelegramUser();
        var text = await DatabaseContext.LocalizedTexts
            .Where(x => x.Language == Language.Russian)
            .Where(x => x.LocalizedTextId == LocalizedTextId.SelectLanguage)
            .Select(x => x.LocalizedText)
            .FirstAsync();

        await Harness.Start();

        // Act

        await _startStateHandler.Handle(user.Id, null, new Infrastructure.StateHandlers.Management.Start.StartState(),
            UniqueText);

        // Assert

        await Harness.ValidateStartStateActivate(text, user.ChatId);
    }
}