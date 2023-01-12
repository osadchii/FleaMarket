using Microsoft.AspNetCore.Mvc;

namespace FleaMarket.Api.Controllers;

[Route("telegram")]
[ApiController]
public class TelegramController : Controller
{
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        return Ok();
    }
}