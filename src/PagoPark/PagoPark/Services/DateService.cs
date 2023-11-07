using System.Globalization;

namespace PagoPark.Services;

public interface IDateService
{
    DateTime DatetimeStartOfWeek { get; }

    (DateTime, DateTime) FirstLastDayOfMonth(DateTime? d = null);
    DateTime FirstSundayOfMonth();
    int GetDayOfWeek(DateTime d);
    string GetDayOfWeekName(DateTime d);
    (int, string) GetMonth(DateTime d);
    (DateTime, DateTime) GetWeekDates(int year, int weekNumber);
    int GetWeekNumber(DateTime date);
    int TotalWeekfrequencyInMonth(int[] weekfrequency);
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

    public DateTime DatetimeStartOfWeek => DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);

    public int GetWeekNumber(DateTime date)
    {
        CalendarWeekRule weekRule = CalendarWeekRule.FirstDay;
        DayOfWeek firstDay = DayOfWeek.Sunday;

        int weekNumber = calendar.GetWeekOfYear(date, weekRule, firstDay);

        return weekNumber;
    }

    public string GetDayOfWeekName(DateTime d) => d.ToString("dddd", culture);

    public int GetDayOfWeek(DateTime d) => (int)d.DayOfWeek;

    public (int, string) GetMonth(DateTime d) => (d.Month, d.ToString("MMMM").ToUpperInvariant());

    public (DateTime, DateTime) GetWeekDates(int year, int weekNumber)
    {
        DateTime startDate = FirstDayOfWeek(year, weekNumber);
        DateTime endDate = startDate.AddDays(6);

        return (startDate, endDate);
    }

    public DateTime FirstSundayOfMonth()
    {
        DateTime primerDiaDelMes = new(DateTime.Now.Year, DateTime.Now.Month, 1);
        return primerDiaDelMes.AddDays(((int)DayOfWeek.Sunday - (int)primerDiaDelMes.DayOfWeek + 7) % 7);
    }

    public (DateTime, DateTime) FirstLastDayOfMonth(DateTime? d = null)
    {
        DateTime date = d is null ? DateTime.Now : d.Value;

        DateTime primerDiaDelMes = new(date.Year, date.Month, 1);
        DateTime ultimoDiaDelMes = primerDiaDelMes.AddMonths(1).AddDays(-1);
        return (primerDiaDelMes, ultimoDiaDelMes);
    }

    public int TotalWeekfrequencyInMonth(int[] weekfrequency)
    {
        int asistencias = 0;
        var dFL = FirstLastDayOfMonth();

        foreach (var item in weekfrequency)
        {
            int diasHastaDiaDeLaSemana = (item - (int)dFL.Item1.DayOfWeek + 7) % 7;
            DateTime primerDiaDeLaSemana = dFL.Item1.AddDays(diasHastaDiaDeLaSemana);
            int diasEnElMes = DateTime.DaysInMonth(dFL.Item1.Year, dFL.Item1.Month);
            asistencias += (diasEnElMes - (primerDiaDeLaSemana.Day - 1) + 6) / 7;
        }
        return asistencias;
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

