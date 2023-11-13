
namespace PagoPark.Models;

public class ShareReport
{
    public string Title { get; set; }
    public string Issued { get; set; }
    public string DatetimeIssue { get; set; }
    public WeekOrMontReport[] ReportItems { get; set; }
    public string Observations { get; set; }
}

public class WeekSharereport : ShareReport
{
    public Tuple<DateTime, DateTime> Week { get; set; }

    public WeekSharereport(string title, string issued, string datetimeissue, WeekOrMontReport[] reportitems, string observation, Tuple<DateTime, DateTime> week)
        : base()
    {
        Title = title;
        Issued = issued;
        DatetimeIssue = datetimeissue;
        ReportItems = reportitems;
        Observations = observation;
        Week = week;
    }
}

public class MonthSharereport : ShareReport
{
    public int Month { get; set; }

    public MonthSharereport(string title, string issued, string datetimeissue, WeekOrMontReport[] reportitems, string observation, int month)
        : base()
    {
        Title = title;
        Issued = issued;
        DatetimeIssue = datetimeissue;
        ReportItems = reportitems;
        Observations = observation;
        Month = month;
    }
}
