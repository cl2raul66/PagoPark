using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PagoPark.Services;
using PagoPark.Views;

namespace PagoPark.ViewModels;

public partial class PgHomeViewModel : ObservableRecipient
{
    readonly IAuthService authServ;
    readonly ILiteDbParkContractServices parkContractServ;
    readonly ILiteDbDailyPaymentLogService dailyPaymentLogServ;
    readonly IDateService dateServ;

    public PgHomeViewModel(IAuthService authService, ILiteDbParkContractServices parkContractServices, ILiteDbDailyPaymentLogService dailyPaymentLogService, IDateService dateService)
    {
        IsActive = true;
        authServ = authService;
        parkContractServ = parkContractServices;
        dailyPaymentLogServ = dailyPaymentLogService;
        dateServ = dateService;

        HasParkContracts = parkContractServ.Any();
        GetAmountcharged();
        _ = GoToSingin();
    }

    [ObservableProperty]
    bool hasParkContracts;

    [ObservableProperty]
    string assists;

    [ObservableProperty]
    string amountCharged;

    [RelayCommand]
    async Task GoToSetPayAllWeek()
    {
        Dictionary<string, object> senderObjects = new() { { "ParkContracts", parkContractServ.GetAll().ToList() } };
        await Shell.Current.GoToAsync($"//{nameof(PgPay)}/{nameof(PgAddPayForAllWeek)}", true, senderObjects);
    }

    #region Extra
    protected override void OnActivated()
    {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register<PgHomeViewModel, string, string>(this, nameof(HasParkContracts), (r, m) =>
        {
            _ = bool.TryParse(m, out bool resul);
            r.HasParkContracts = resul;
        });
    }

    async Task GoToSingin()
    {
        if (!authServ.LoadCurrentUser())
        {
            await Shell.Current.GoToAsync(nameof(PgSingIn), true);
        }
    }

    void GetAmountcharged()
    {
        var dFL = dateServ.FirstLastDayOfMonth();
        var amountcollected = dailyPaymentLogServ.AmountCollected(dFL.Item1, dFL.Item2);
        var ReservedDays = parkContractServ.GetAll().Select(x => (x.WeekFrequency, x.PayPerFrequency));
        double totalPay = 0;
        foreach (var (WeekFrequency, PayPerFrequency) in ReservedDays)
        {
            totalPay += PayPerFrequency * dateServ.TotalDays(WeekFrequency);
        }

        AmountCharged = $"{amountcollected:0.00} / {totalPay:0.00}";
    }
    #endregion
}
