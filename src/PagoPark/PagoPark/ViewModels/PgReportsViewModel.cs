using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PagoPark.Models;
using PagoPark.Services;
using PagoPark.Tools;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace PagoPark.ViewModels;

[QueryProperty(nameof(Title), nameof(Title))]
public partial class PgReportsViewModel : ObservableObject
{
    readonly string[] options = { "By week", "By month", "All this year" };
    readonly IDateService dateServ;
    readonly ILiteDbDailyPaymentLogService dailyPaymentLogServ;
    readonly ILiteDbParkContractServices parkContractServ;
    readonly IAuthService authServ;

    string Observations = string.Empty;

    public PgReportsViewModel(IDateService dateService, ILiteDbDailyPaymentLogService dailyPaymentLogService, ILiteDbParkContractServices parkContractServices, IAuthService authService)
    {
        dateServ = dateService;
        dailyPaymentLogServ = dailyPaymentLogService;
        parkContractServ = parkContractServices;
        authServ = authService;
    }

    [ObservableProperty]
    string title;

    #region Week
    [ObservableProperty]
    int selectedWeek = 1;

    [ObservableProperty]
    int lastCurrentweek = 2;

    [ObservableProperty]
    string startDate;

    [ObservableProperty]
    string endDate;

    [ObservableProperty]
    ObservableCollection<WeekOrMontReport> weekReports;
    #endregion

    [ObservableProperty]
    double amountCharged;

    #region Month
    [ObservableProperty]
    int selectedMonth = 1;

    [ObservableProperty]
    int lastCurrentMonth = 2;

    [ObservableProperty]
    ObservableCollection<WeekOrMontReport> montReports;
    #endregion

    #region Year
    [ObservableProperty]
    ObservableCollection<WeekOrMontReport> annualReport;
    #endregion

    [RelayCommand]
    async Task Close() => await Shell.Current.GoToAsync("..", true);

    [RelayCommand]
    async Task Sharereport()
    {
        (string, ShareReport) fn = Title switch
        {
            "By week" => ("Weekly report.pdf", new() { Title = "Weekly report", Issued = authServ.currentUser.Username, DatetimeIssue = DateTime.Now.ToString(), ReportItems = WeekReports.ToArray(), Observations= Observations }),
            "By month" => ("Monthly report.pdf", new()),
            _ => ($"Annual report {DateTime.Now.Year}.pdf", new())
        };
        string file = Path.Combine(FileSystem.CacheDirectory, fn.Item1);

        var document = new ReportDocument(fn.Item2) as IDocument;
        document.GeneratePdf(file);

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = $"Share - {Path.GetFileNameWithoutExtension(file)}",
            File = new ShareFile(file)
        });
    }

    #region Extra
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(Title))
        {
            switch (Title)
            {
                case "By week":
                    int getweeknumber = dateServ.GetWeekNumber(DateTime.Now);
                    LastCurrentweek = getweeknumber > 1 ? getweeknumber - 1 : 1;
                    SelectedWeek = LastCurrentweek;
                    break;
                case "By month":
                    LastCurrentMonth = DateTime.Now.Month > 1 ? DateTime.Now.Month - 1 : 1;
                    SelectedMonth = LastCurrentMonth;
                    break;
                case "All this year":
                    var currentMonth = DateTime.Now.Month > 1 ? DateTime.Now.Month - 1 : 1;
                    AnnualReport = new();
                    for (int i = 1; i <= currentMonth; i++)
                    {
                        var (sDate, eDate) = dateServ.FirstLastDayOfMonth(new DateTime(DateTime.Now.Year, i, 1));
                        var dailyPaymentLogs = dailyPaymentLogServ.GetByDates(sDate, eDate);
                        int absence = 0;
                        double collected = 0;
                        foreach (var item in parkContractServ.GetAll())
                        {
                            absence += dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && (x.Amount == null || x.Amount == 0) && (x.Note != null && x.Note.Contains("Not presented"))).Count();
                            collected += dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && x.Amount != null && x.Amount > 0).Select(x => x.Amount)?.Sum() ?? 0;
                        }
                        string monthName = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM", CultureInfo.CurrentCulture).ToUpper();
                        AnnualReport.Add(new(monthName, absence, collected));
                    }
                    AmountCharged = AnnualReport.Select(x => x.TotalCollected).Sum();
                    break;
                default:
                    break;
            }
        }

        if (e.PropertyName == nameof(SelectedWeek))
        {
            var (sDate, eDate) = dateServ.GetWeekDates(DateTime.Now.Year, SelectedWeek);
            StartDate = sDate.ToString("dd/MM/yyyy");
            EndDate = eDate.ToString("dd/MM/yyyy");

            var dailyPaymentLogs = dailyPaymentLogServ.GetByDates(sDate, eDate);
            WeekReports = new();
            foreach (var item in parkContractServ.GetAll())
            {
                string vehicle = item.ToString();
                int absence = dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && (x.Amount == null || x.Amount == 0) && (x.Note != null && x.Note.Contains("Not presented"))).Count();
                double collected = dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && x.Amount != null && x.Amount > 0).Select(x => x.Amount)?.Sum() ?? 0;
                WeekReports.Add(new(vehicle, absence, collected));

                foreach (var item2 in dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && !string.IsNullOrEmpty(x.Note)))
                {
                    string note = item2.Note.Replace("Not presented;", string.Empty);
                    if (string.IsNullOrEmpty(note))
                    {
                        break;
                    }
                    Observations += $"{item2.PaymentDate:dd/MM/yyyy} - {item}: {note}\n";
                }                
            }

            AmountCharged = WeekReports.Select(x => x.TotalCollected).Sum();
            //Observations = Observations.Replace("Not presented; ", string.Empty);
        }

        if (e.PropertyName == nameof(SelectedMonth))
        {
            var (sDate, eDate) = dateServ.FirstLastDayOfMonth(new DateTime(DateTime.Now.Year, SelectedMonth, 1));
            var dailyPaymentLogs = dailyPaymentLogServ.GetByDates(sDate, eDate);
            MontReports = new();

            foreach (var item in parkContractServ.GetAll())
            {
                string vehicle = item.ToString();
                int absence = dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && (x.Amount == null || x.Amount == 0) && (x.Note != null && x.Note.Contains("Not presented"))).Count();
                double collected = dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && x.Amount != null && x.Amount > 0).Select(x => x.Amount)?.Sum() ?? 0;
                MontReports.Add(new(vehicle, absence, collected));
            }

            AmountCharged = MontReports.Select(x => x.TotalCollected).Sum();
        }
    }
    #endregion
}
