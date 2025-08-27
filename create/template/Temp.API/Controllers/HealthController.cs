using Microsoft.AspNetCore.Mvc;

namespace Temp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello, World!");
    }
}
