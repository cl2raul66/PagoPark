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
    //readonly string[] options = { "By week", "By month", "All this year" };
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

        Task.Run(() =>
        {
            foreach (var f in Directory.GetFiles(FileSystem.CacheDirectory, "*.pdf"))
            {
                File.Delete(f);
            }
        });
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
    ObservableCollection<Report> weekReports;
    #endregion

    [ObservableProperty]
    double amountCharged;

    #region Month
    [ObservableProperty]
    int selectedMonth = 1;

    [ObservableProperty]
    int lastCurrentMonth = 2;

    [ObservableProperty]
    ObservableCollection<Report> montReports;
    #endregion

    #region Year
    [ObservableProperty]
    ObservableCollection<Report> annualReport;
    #endregion

    [RelayCommand]
    async Task Close() => await Shell.Current.GoToAsync("..", true);

    [RelayCommand]
    async Task Sharereport()
    {
        (string t, ReportDocument d) = Title switch
        {
            "By week" => ($"Weekly report {StartDate} to {EndDate}", new ReportDocument("Weekly report", authServ.currentUser.Username, DateTime.Now.ToString("F"), WeekReports.ToArray(), Observations, dateServ.GetWeekDates(DateTime.Now.Year, SelectedWeek).ToTuple())),
            "By month" => ($"Monthly report {dateServ.GetMonthNameByNumber(SelectedMonth)}", new ReportDocument("Monthly report", authServ.currentUser.Username, DateTime.Now.ToString("F"), MontReports.ToArray(), Observations, new DateTime(DateTime.Now.Year, SelectedMonth, 1))),
            _ => ($"Annual report {DateTime.Now.Year}", new ReportDocument("Annual report", authServ.currentUser.Username, DateTime.Now.ToString("F"), AnnualReport.ToArray(), Observations, DateTime.Now.Year))
        };
        string file = Path.Combine(FileSystem.CacheDirectory, t + ".pdf");

        var document = d as IDocument;
        document.GeneratePdf(file);

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = $"Share - {t}",
            File = new ShareFile(file)
        });
    }

    #region Extra
    protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
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
                        Dictionary<string, string> observationDetail = new();
                        foreach (var item in parkContractServ.GetAll())
                        {
                            absence += dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && (x.Amount == null || x.Amount == 0) && x.Note != null && x.Note.Contains("Not presented")).Count();
                            collected += dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && x.Amount != null && x.Amount > 0).Select(x => x.Amount)?.Sum() ?? 0;
                            foreach (var item2 in dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && !string.IsNullOrEmpty(x.Note)))
                            {
                                if (observationDetail.Any())
                                {
                                    observationDetail[item.VehicleClient.ToString()] += $"{item2.PaymentDate:dd/MM/yyyy}: {item2.Note}\n";
                                    break;
                                }
                                observationDetail.Add(item.VehicleClient.ToString(), $"{item2.PaymentDate:dd/MM/yyyy}: {item2.Note}\n");
                            }
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
            StartDate = sDate.ToString("dd-MM-yyyy");
            EndDate = eDate.ToString("dd-MM-yyyy");

            var dailyPaymentLogs = dailyPaymentLogServ.GetByDates(sDate, eDate);
            WeekReports = new();
            foreach (var item in parkContractServ.GetAll())
            {
                int absence = await Task.Run(() => dailyPaymentLogs.Count(x => x.ParkContractId == item.Id && (x.Amount == null || x.Amount == 0) && x.Note != null && x.Note.Contains("Not presented")));
                double collected = await Task.Run(() => dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && x.Amount != null && x.Amount > 0).Select(x => x.Amount)?.Sum() ?? 0);
                WeekReports.Add(new(item.ToString(), absence, collected));

                foreach (var item2 in dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && !string.IsNullOrEmpty(x.Note)))
                {
                    Observations += $"{item2.PaymentDate:dd/MM/yyyy} - {item.VehicleClient}: {item2.Note}\n";
                }
            }

            AmountCharged = WeekReports.Sum(x => x.TotalCollected);
        }

        if (e.PropertyName == nameof(SelectedMonth))
        {
            var (sDate, eDate) = dateServ.FirstLastDayOfMonth(new DateTime(DateTime.Now.Year, SelectedMonth, 1));
            var dailyPaymentLogs = dailyPaymentLogServ.GetByDates(sDate, eDate);
            MontReports = new();

            foreach (var item in parkContractServ.GetAll())
            {
                int absence = await Task.Run(() => dailyPaymentLogs.Count(x => x.ParkContractId == item.Id && (x.Amount == null || x.Amount == 0) && (x.Note != null && x.Note.Contains("Not presented"))));
                double collected = await Task.Run(() => dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && x.Amount != null && x.Amount > 0).Select(x => x.Amount)?.Sum() ?? 0);
                MontReports.Add(new(item.ToString(), absence, collected));

                foreach (var item2 in dailyPaymentLogs.Where(x => x.ParkContractId == item.Id && !string.IsNullOrEmpty(x.Note)))
                {
                    Observations += $"{item2.PaymentDate:dd/MM/yyyy} - {item.VehicleClient}: {item2.Note}\n";
                }
            }

            AmountCharged = MontReports.Select(x => x.TotalCollected).Sum();
        }
    }
    #endregion
}
