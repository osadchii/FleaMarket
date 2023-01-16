using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Services.LocalizedText;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace FleaMarket.Tests.Services.LocalizedTextService;

public class WhenGetLocalizedText : TestContext
{
    private readonly ILocalizedTextService _localizedTextService;

    public WhenGetLocalizedText(TestWebApplicationFactory factory) : base(factory)
    {
        _localizedTextService = Services.GetRequiredService<ILocalizedTextService>();
    }

    [Fact]
    public async Task ShouldReturnValue()
    {
        // Arrange

        var localizedText = await CreateLocalizedText(entity =>
        {
            entity.Language = Language.Russian;
            entity.LocalizedTextId = LocalizedTextId.SelectLanguage;
        });
        
        // Act

        var value = await _localizedTextService.GetText(LocalizedTextId.SelectLanguage, Language.Russian);
        
        // Assert
        
        value.ShouldNotBeEmpty();
        value.ShouldBe(localizedText.LocalizedText);
    }
}