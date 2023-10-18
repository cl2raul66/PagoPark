using System.Globalization;

namespace PagoPark.Services;

public interface IDateService
{
    string GetDayOfWeek(DateTime d);
    int GetNumberOfWeek(DateTime d);
    (DateTime, DateTime) GetWeekDates(int year, int weekNumber);
    int GetWeekNumber(DateTime date);
}

public class DateService : IDateService
{
    readonly CultureInfo culture;
    readonly Calendar calendar;

    public DateService()
    {
        this.culture = CultureInfo.CurrentCulture;
        calendar = this.culture.Calendar;
    }

    public int GetWeekNumber(DateTime date)
    {        
        CalendarWeekRule weekRule = CalendarWeekRule.FirstDay;
        DayOfWeek firstDay = DayOfWeek.Sunday;

        int weekNumber = calendar.GetWeekOfYear(date, weekRule, firstDay);

        return weekNumber;
    }

    public string GetDayOfWeek(DateTime d) => d.ToString("dddd", culture);

    public int GetNumberOfWeek(DateTime d) => (int)d.DayOfWeek;

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

