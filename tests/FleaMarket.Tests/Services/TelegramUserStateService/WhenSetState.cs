using FleaMarket.Data.Entities;
using FleaMarket.Infrastructure.Extensions;
using FleaMarket.Infrastructure.Services.TelegramUserStateService;
using FleaMarket.Infrastructure.StateHandlers.Management.AddBot;
using FleaMarket.Infrastructure.StateHandlers.Management.AddBotConfirmation;
using FleaMarket.Infrastructure.StateHandlers.Management.MainMenu;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace FleaMarket.Tests.Services.TelegramUserStateService;

public class WhenSetState : TestContext
{
    private readonly ITelegramUserStateService _telegramUserStateService;

    public WhenSetState(TestWebApplicationFactory factory) : base(factory)
    {
        _telegramUserStateService = Services.GetRequiredService<ITelegramUserStateService>();
    }

    [Fact]
    public async Task ShouldBeUpdated()
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

        var addBotConfirmationState = new AddBotConfirmationState
        {
            Token = UniqueText
        };
        userState = new TelegramUserState(addBotConfirmationState.GetType().Name, addBotConfirmationState.ToJson());
        
        // Act

        await _telegramUserStateService.SetState(user.Id, null, userState);
        
        // Arrange

        userStateEntity = await DatabaseContext.TelegramUserStates
            .Where(x => x.TelegramUserId == user.Id)
            .FirstOrDefaultAsync(x => x.TelegramBotId == null);

        userStateEntity.ShouldNotBeNull();

        var state = userStateEntity.State.FromJson<TelegramUserState>();
        state.ShouldNotBeNull();

        var deserializedState = state.StateData.FromJson<AddBotConfirmationState>();
        deserializedState.ShouldNotBeNull();
        deserializedState.Token.ShouldBe(addBotConfirmationState.Token);
    }

    [Fact]
    public async Task ShouldBeSetForManagementBot()
    {
        // Arrange

        var user = await CreateTelegramUser();
        var mainMenuState = MainMenuState.Default;
        var userState = new TelegramUserState(mainMenuState.GetType().Name, mainMenuState.ToJson());

        // Act

        await _telegramUserStateService.SetState(user.Id, null, userState);

        // Assert

        var userStateEntity = await DatabaseContext.TelegramUserStates
            .Where(x => x.TelegramUserId == user.Id)
            .FirstOrDefaultAsync(x => x.TelegramBotId == null);

        userStateEntity.ShouldNotBeNull();

        var state = userStateEntity.State.FromJson<TelegramUserState>();
        state.ShouldNotBeNull();

        var deserializedState = state.StateData.FromJson<MainMenuState>();
        deserializedState.ShouldNotBeNull();
    }

    [Fact]
    public async Task ShouldBeSetForRetailerBot()
    {
        // Arrange

        var user = await CreateTelegramUser();
        var telegramBot = await CreateTelegramBot(user.Id);
        var mainMenuState = MainMenuState.Default;
        var userState = new TelegramUserState(mainMenuState.GetType().Name, mainMenuState.ToJson());

        // Act

        await _telegramUserStateService.SetState(user.Id, telegramBot.Id, userState);

        // Assert

        var userStateEntity = await DatabaseContext.TelegramUserStates
            .Where(x => x.TelegramUserId == user.Id)
            .FirstOrDefaultAsync(x => x.TelegramBotId == telegramBot.Id);

        userStateEntity.ShouldNotBeNull();

        var state = userStateEntity.State.FromJson<TelegramUserState>();
        state.ShouldNotBeNull();

        var deserializedState = state.StateData.FromJson<MainMenuState>();
        deserializedState.ShouldNotBeNull();
    }
}