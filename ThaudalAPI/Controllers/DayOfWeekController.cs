using DayOfWeekService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ThaudalAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class DayOfWeekController : ControllerBase
{
    private readonly IDateService _service;

    public DayOfWeekController(IDateService service)
    {
        _service = service;
    }

    [HttpGet]
    public bool IsItFriday()
    {
        return _service.IsItFridayInternal(DateTime.Now);
    }
}