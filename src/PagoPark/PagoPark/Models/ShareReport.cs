
namespace PagoPark.Models;

public class ShareReport
{
    public string Title { get; set; }
    public string Issued { get; set; }
    public string DatetimeOfIssue { get; set; }
    public WeekOrMontReport[] ReportItems { get; set; }
    public string Observations { get; set; }
}
