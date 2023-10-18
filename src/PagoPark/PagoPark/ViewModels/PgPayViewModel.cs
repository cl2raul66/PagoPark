using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PagoPark.Models;
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
    ObservableCollection<DailyPaymentLog> thisWeek;

    [ObservableProperty]
    DailyPaymentLog currentWeekDay;

    [ObservableProperty]
    PayContract selectedContract;

    [RelayCommand]
    async Task GoToSetPay()
    {
        Dictionary<string, object> senderObject = new() { { nameof(SelectedContract), SelectedContract } };
        await Shell.Current.GoToAsync(nameof(PgAddPay), true, senderObject);
    }

    #region Extra
    protected override void OnActivated()
    {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register<PgPayViewModel, PayContract, string>(this, nameof(PgAddPay), (r, m) =>
        {
            GetThisweek();
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
            SelectedContract = null;
        }
    }

    void GetThisweek()
    {
        if (parkContractServ.Any())
        {
            var getthisweek = dailyPaymentLogServ.GetThisWeek();
            if (!getthisweek.Any())
            {
                var dateBeginEnd = dateServ.GetWeekDates(DateTime.Now.Year, dateServ.GetWeekNumber(DateTime.Now));
                for (DateTime date = dateBeginEnd.Item1; date < dateBeginEnd.Item2; date = date.AddDays(1))
                {
                    var getbyweeknumber = parkContractServ.GetByWeekNumber(dateServ.GetNumberOfWeek(date));
                    if (getbyweeknumber.Any())
                    {
                        foreach (var item in getbyweeknumber)
                        {
                            dailyPaymentLogServ.Insert(new DailyPaymentLog(date, item.Id ?? string.Empty));
                        }
                    }
                }
                getthisweek = dailyPaymentLogServ.GetThisWeek();
            }
            ThisWeek = new(getthisweek);
            CurrentWeekDay = ThisWeek[(int)DateTime.Now.DayOfWeek];
        }
    }
    #endregion
}
