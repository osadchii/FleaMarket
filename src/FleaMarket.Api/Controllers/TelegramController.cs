﻿using FleaMarket.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace FleaMarket.Api.Controllers;

[Route("telegram/{token}")]
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