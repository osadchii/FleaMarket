using FleaMarket.Data.Constants;
using FleaMarket.Infrastructure.Services;
using FleaMarket.Infrastructure.Services.UpdateHandlers;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace FleaMarket.Api.Controllers;

[Route($"{ApplicationConstant.TelegramController}/{{token}}")]
[ApiController]
public class TelegramController : Controller
{
    private readonly IUpdateHandleService _updateHandleService;

    public TelegramController(IUpdateHandleService updateHandleService)
    {
        _updateHandleService = updateHandleService;
    }

    [HttpPost]
    public async Task<IActionResult> Update(string token, [FromBody] Update update)
    {
        await _updateHandleService.Handle(token, update);
        return Ok();
    }
}