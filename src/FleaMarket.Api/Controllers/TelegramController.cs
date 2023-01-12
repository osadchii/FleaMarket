using Microsoft.AspNetCore.Mvc;

namespace FleaMarket.Api.Controllers;

[Route("telegram")]
[ApiController]
public class TelegramController : Controller
{
    private readonly ILogger<TelegramController> _logger;

    public TelegramController(ILogger<TelegramController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        _logger.LogInformation("OK");
        return Ok();
    }
}