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
    }

    [ObservableProperty]
    ObservableCollection<DateTime> thisWeek = new();

    [ObservableProperty]
    DateTime currentWeekDay;

    [ObservableProperty]
    ObservableCollection<DailyPaymentLogView> paymentlogs;

    [ObservableProperty]
    DailyPaymentLogView selectedPaymentLog;

    [RelayCommand]
    async Task GoToSetPay()
    {
        Dictionary<string, object> senderObject = new() { { nameof(SelectedPaymentLog), SelectedPaymentLog } };
        await Shell.Current.GoToAsync(nameof(PgAddPay), true, senderObject);
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
                Paymentlogs = new(dailyPaymentLogServ.GetByDate(CurrentWeekDay).Select(x => new DailyPaymentLogView(parkContractServ, x)));
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
            Paymentlogs = new(dailyPaymentLogServ.GetByDate(CurrentWeekDay).Select(x => new DailyPaymentLogView(parkContractServ, x)));
        }
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
                        var getbyweeknumber = parkContractServ.GetByWeekNumber(dateServ.GetNumberOfWeek(date));
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
    #endregion
}
