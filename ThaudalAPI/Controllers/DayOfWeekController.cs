using Microsoft.AspNetCore.Mvc;

namespace ThaudalAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class DayOfWeekController: ControllerBase
{
    [HttpGet]
    public bool IsItFriday()
    {
        return IsItFridayInternal(DateTime.Now);
    }

    public bool IsItFridayInternal(DateTime dateTime)
    {
        return dateTime.DayOfWeek.Equals(DayOfWeek.Friday);
    }
}