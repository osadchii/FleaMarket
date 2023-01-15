using FleaMarket.Data.Constants;
using FleaMarket.Infrastructure.Handlers.Telegram.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace FleaMarket.Api.Controllers;

[Route($"{ApplicationConstant.TelegramController}/{{token}}")]
[ApiController]
public class TelegramController : Controller
{
    private readonly IMediator _mediator;

    public TelegramController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Update(string token, [FromBody] Update update)
    {
        var command = new ApplyUpdate.Command(token, update);
        await _mediator.Send(command);
        return Ok();
    }
}