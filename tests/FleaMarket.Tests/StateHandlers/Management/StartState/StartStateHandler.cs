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

        // Act

        await _startStateHandler.Activate(user.Id, null,
            new Infrastructure.StateHandlers.Management.Start.StartState());

        // Assert

        await this.ValidateStartStateActivate(user.ChatId);
    }

    [Fact]
    public async Task ShouldSetLanguage()
    {
        // Arrange

        var user = await CreateTelegramUser();

        // Act

        await _startStateHandler.Handle(user.Id, null, new Infrastructure.StateHandlers.Management.Start.StartState(),
            Language.Russian.ToString());

        // Assert

        await this.ValidateMainMenuStateActivate(user.ChatId);

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

        // Act

        await _startStateHandler.Handle(user.Id, null, new Infrastructure.StateHandlers.Management.Start.StartState(),
            UniqueText);

        // Assert

        await this.ValidateStartStateActivate(user.ChatId);
    }
}