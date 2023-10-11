using System.Globalization;

namespace PagoPark.Services;

public interface IDateService
{
    (DateTime, DateTime) GetWeekDates(int year, int weekNumber);
    int GetWeekNumber(DateTime date);
}

public class DateService : IDateService
{
    readonly CultureInfo culture;

    public DateService()
    {
        this.culture = CultureInfo.CurrentCulture;
    }

    public int GetWeekNumber(DateTime date)
    {
        Calendar calendar = this.culture.Calendar;
        CalendarWeekRule weekRule = CalendarWeekRule.FirstDay;
        DayOfWeek firstDay = DayOfWeek.Sunday;

        int weekNumber = calendar.GetWeekOfYear(date, weekRule, firstDay);

        return weekNumber;
    }

    public (DateTime, DateTime) GetWeekDates(int year, int weekNumber)
    {
        DateTime startDate = FirstDayOfWeek(year, weekNumber);
        DateTime endDate = startDate.AddDays(6);

        return (startDate, endDate);
    }

    private DateTime FirstDayOfWeek(int year, int weekNumber)
    {
        DateTime firstDayOfYear = new(year, 1, 1);
        int days = (weekNumber - 1) * 7;
        DateTime date = firstDayOfYear.AddDays(days);

        DayOfWeek firstDayOfWeek = this.culture.DateTimeFormat.FirstDayOfWeek;
        while (date.DayOfWeek != firstDayOfWeek)
            date = date.AddDays(-1);

        return date;
    }
}

