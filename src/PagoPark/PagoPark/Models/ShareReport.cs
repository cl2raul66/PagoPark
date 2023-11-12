
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

    public WeekSharereport(ShareReport shareReport, Tuple<DateTime, DateTime> week)
        : base()
    {
        Title = shareReport.Title;
        Issued = shareReport.Issued;
        DatetimeIssue = shareReport.DatetimeIssue;
        ReportItems = shareReport.ReportItems;
        Observations = shareReport.Observations;
        Week = week;
    }
}
