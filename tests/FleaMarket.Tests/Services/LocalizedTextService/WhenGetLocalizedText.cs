using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Services.LocalizedText;
using Microsoft.EntityFrameworkCore;
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
        
        var text = await GetLocalizedText(LocalizedTextId.SelectLanguage, Language.Russian);
        
        // Act

        var value = await _localizedTextService.GetText(LocalizedTextId.SelectLanguage, Language.Russian);
        
        // Assert
        
        value.ShouldNotBeEmpty();
        value.ShouldBe(text);
    }
}