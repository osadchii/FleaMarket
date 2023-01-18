using FleaMarket.Data.Entities;
using FleaMarket.Infrastructure.Extensions;
using FleaMarket.Infrastructure.Services.TelegramUserStateService;
using FleaMarket.Infrastructure.StateHandlers.Management.MainMenu;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace FleaMarket.Tests.Services.TelegramUserStateService;

public class WhenGetState : TestContext
{
    private readonly ITelegramUserStateService _telegramUserStateService;

    public WhenGetState(TestWebApplicationFactory factory) : base(factory)
    {
        _telegramUserStateService = Services.GetRequiredService<ITelegramUserStateService>();
    }

    [Fact]
    public async Task ShouldBeNullWhenUserHasNoState()
    {
        // Arrange

        var user = await CreateTelegramUser();

        // Act

        var userStateFromService = await _telegramUserStateService.GetState(user.Id, null);

        // Assert

        userStateFromService.ShouldBeNull();
    }

    [Fact]
    public async Task ShouldBeGetForManagementBot()
    {
        // Arrange

        var user = await CreateTelegramUser();
        var mainMenuState = MainMenuState.Default;
        var userState = new TelegramUserState(mainMenuState.GetType().Name, mainMenuState.ToJson());

        var userStateEntity = new TelegramUserStateEntity
        {
            TelegramUserId = user.Id,
            State = userState.ToJson()
        };

        await DatabaseContext.AddAsync(userStateEntity);
        await DatabaseContext.SaveChangesAsync();

        // Act

        var userStateFromService = await _telegramUserStateService.GetState(user.Id, null);

        // Assert

        userStateFromService.ShouldNotBeNull();

        userStateFromService.Name.ShouldBe(userState.Name);
        userStateFromService.StateData.ShouldBe(userState.StateData);
    }

    [Fact]
    public async Task ShouldBeGetForRetailerBot()
    {
        // Arrange

        var user = await CreateTelegramUser();
        var bot = await CreateTelegramBot(user.Id);

        var mainMenuState = MainMenuState.Default;
        var userState = new TelegramUserState(mainMenuState.GetType().Name, mainMenuState.ToJson());

        var userStateEntity = new TelegramUserStateEntity
        {
            TelegramUserId = user.Id,
            TelegramBotId = bot.Id,
            State = userState.ToJson()
        };

        await DatabaseContext.AddAsync(userStateEntity);
        await DatabaseContext.SaveChangesAsync();

        // Act

        var userStateFromService = await _telegramUserStateService.GetState(user.Id, bot.Id);

        // Assert

        userStateFromService.ShouldNotBeNull();

        userStateFromService.Name.ShouldBe(userState.Name);
        userStateFromService.StateData.ShouldBe(userState.StateData);
    }
}