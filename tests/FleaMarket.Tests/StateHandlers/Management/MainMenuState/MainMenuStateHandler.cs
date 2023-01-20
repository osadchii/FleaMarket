using FleaMarket.Infrastructure.StateHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Tests.StateHandlers.Management.MainMenuState;

public class MainMenuStateHandler : TestContext
{
    private readonly IStateHandler<Infrastructure.StateHandlers.Management.MainMenu.MainMenuState, string> _mainMenuStateHandler;
    
    public MainMenuStateHandler(TestWebApplicationFactory factory) : base(factory)
    {
        _mainMenuStateHandler = Services.GetRequiredService<IStateHandler<Infrastructure.StateHandlers.Management.MainMenu.MainMenuState, string>>();
    }

    [Fact]
    public async Task ShouldBeActivated()
    {
        // Arrange

        var user = await CreateTelegramUser();

        // Act

        await _mainMenuStateHandler.Activate(user.Id, null,
            Infrastructure.StateHandlers.Management.MainMenu.MainMenuState.Default);

        // Assert

        await this.ValidateMainMenuStateActivate(user.ChatId);
    }

    [Fact]
    public async Task ShouldBeReactivated()
    {
        // Arrange

        var user = await CreateTelegramUser();

        // Act

        await _mainMenuStateHandler.Handle(user.Id, null,
            Infrastructure.StateHandlers.Management.MainMenu.MainMenuState.Default, UniqueText);

        // Assert

        await this.ValidateMainMenuStateActivate(user.ChatId);
    }
}