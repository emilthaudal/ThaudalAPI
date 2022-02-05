using DayOfWeekService.Interfaces;

namespace DayOfWeekService.Service;

public class DateService: IDateService
{
    public bool IsItFridayInternal(DateTime dateTime)
    {
        return dateTime.DayOfWeek.Equals(DayOfWeek.Friday);
    }
}