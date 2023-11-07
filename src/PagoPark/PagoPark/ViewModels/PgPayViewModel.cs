using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PagoPark.Models;
using PagoPark.Models.Observables;
using PagoPark.Services;
using PagoPark.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PagoPark.ViewModels;

public partial class PgPayViewModel : ObservableRecipient
{
    readonly ILiteDbParkContractServices parkContractServ;
    readonly ILiteDbDailyPaymentLogService dailyPaymentLogServ;
    readonly IDateService dateServ;

    public PgPayViewModel(ILiteDbParkContractServices parkContractServices, ILiteDbDailyPaymentLogService dailyPaymentLogService, IDateService dateService)
    {
        IsActive = true;
        parkContractServ = parkContractServices;
        dailyPaymentLogServ = dailyPaymentLogService;
        dateServ = dateService;

        GetThisweek();
        GetNotPresented();
    }

    [ObservableProperty]
    ObservableCollection<DateTime> thisWeek = new();

    [ObservableProperty]
    DateTime currentWeekDay;

    [ObservableProperty]
    ObservableCollection<DailyPaymentLogView> paymentlogs;

    [ObservableProperty]
    DailyPaymentLogView selectedPaymentLog;

    //[ObservableProperty]
    //string unpaid;

    [ObservableProperty]
    string notPresented;

    [ObservableProperty]
    bool enableSetNotpresented;

    [RelayCommand]
    async Task GoToSetPay()
    {
        Dictionary<string, object> senderObjects = new() { { nameof(SelectedPaymentLog), SelectedPaymentLog } };
        await Shell.Current.GoToAsync(nameof(PgAddPay), true, senderObjects);
    }

    [RelayCommand]
    async Task GoToSetPayAllWeek()
    {
        Dictionary<string, object> senderObjects = new() { { "ParkContracts", parkContractServ.GetAll().ToList() } };
        await Shell.Current.GoToAsync(nameof(PgAddPayForAllWeek), true, senderObjects);
    }

    [RelayCommand]
    void SetNotpresented()
    {
        DailyPaymentLog log = new(SelectedPaymentLog.PaymentDate, SelectedPaymentLog.ParkContractId, SelectedPaymentLog.RecordDate, SelectedPaymentLog.Amount, SelectedPaymentLog.Note) { Id = SelectedPaymentLog.Id, Note = "Not presented; " + SelectedPaymentLog.Note };
        if (dailyPaymentLogServ.Upsert(log))
        {
            GetNotPresented();
            WeakReferenceMessenger.Default.Send(bool.TrueString, "GetAssists");
        }
    }

    #region Extra
    protected override void OnActivated()
    {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register<PgPayViewModel, DailyPaymentLogView, string>(this, nameof(PgAddPay), (r, m) =>
        {
            DailyPaymentLog log = new(m.PaymentDate, m.ParkContractId, m.RecordDate, m.Amount, m.Note)
            {
                Id = m.Id
            };
            if (dailyPaymentLogServ.Upsert(log))
            {
                GetPaymentlogs();
                GetNotPresented();
                WeakReferenceMessenger.Default.Send(bool.TrueString, "GetAmountcharged");
                //GetUnpaid();
            }
        });
        WeakReferenceMessenger.Default.Register<PgPayViewModel, Tuple<string, double, string>, string>(this, nameof(PgAddPayForAllWeek), (r, m) =>
        {
            if (m is not null)
            {
                var GetDailypaymentlog = dailyPaymentLogServ.GetByParkContractId(m.Item1);
                double amountPart = m.Item2 / GetDailypaymentlog.Count();
                foreach (var item in GetDailypaymentlog)
                {
                    DailyPaymentLog paymentLog = item;
                    paymentLog.Amount = amountPart;
                    paymentLog.Note = m.Item3;
                    dailyPaymentLogServ.Upsert(paymentLog);
                }
                GetPaymentlogs();
            }
        });
        WeakReferenceMessenger.Default.Register<PgPayViewModel, string, string>(this, nameof(PgManageContracts), (r, m) =>
        {
            if (m is not null && bool.Parse(m))
            {
                GetThisweek();
            }
        });
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(CurrentWeekDay))
        {
            SelectedPaymentLog = null;
            GetPaymentlogs();
        }
    }

    void GetPaymentlogs()
    {
        Paymentlogs = new(dailyPaymentLogServ.GetByDate(CurrentWeekDay).Select(x => new DailyPaymentLogView(parkContractServ, x)));
    }

    void GetThisweek()
    {
        if (parkContractServ.Any())
        {
            var dateBeginEnd = dateServ.GetWeekDates(DateTime.Now.Year, dateServ.GetWeekNumber(DateTime.Now));
            bool hasPaymentLog = dailyPaymentLogServ.GetThisWeek()?.Any() ?? false;
            if (ThisWeek.Count < 7)
            {
                for (DateTime date = dateBeginEnd.Item1; date <= dateBeginEnd.Item2; date = date.AddDays(1))
                {
                    ThisWeek.Add(date);
                    if (!hasPaymentLog)
                    {
                        var getbyweeknumber = parkContractServ.GetByWeekNumber(dateServ.GetDayOfWeek(date));
                        foreach (var item in getbyweeknumber)
                        {
                            dailyPaymentLogServ.Upsert(new(date, item.Id));
                        }
                    }
                }
            }
            CurrentWeekDay = ThisWeek[(int)DateTime.Now.DayOfWeek];
        }
    }

    void GetNotPresented()
    {
        var notpresented = dailyPaymentLogServ.GetByDates(dateServ.DatetimeStartOfWeek, CurrentWeekDay.Date).Where(log => (log.Amount == null || log.Amount == 0) && (log.Note != null && log.Note.Contains("Not presented")));
        if (ThisWeek.First().Date == CurrentWeekDay.Date && CurrentWeekDay.Date.Hour <= 19)
        {
            NotPresented = "The day is not over!";
            return;
        }
        if (notpresented is null || !notpresented.Any())
        {
            NotPresented = "No absences yet!";
            return;
        }
        foreach (var item in notpresented)
        {
            NotPresented += parkContractServ.GetById(item.ParkContractId).VehicleClient + ", ";
        }
        NotPresented = NotPresented.TrimEnd(new char[] { ',', ' ' });
    }
    #endregion
}
