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

    public PgHomeViewModel(IAuthService authService, ILiteDbParkContractServices parkContractServices)
    {
        IsActive = true;
        authServ = authService;
        parkContractServ = parkContractServices;

        HasParkContracts = parkContractServ.Any();
        _ = GoToSingin();
    }

    [ObservableProperty]
    bool hasParkContracts;

    //[RelayCommand]
    //async Task GoToPay()
    //{
    //    var send = new Dictionary<string, object> { { nameof(PgAddPay), vehiclesServ.GetAll().Reverse().ToArray() } };
    //    await Shell.Current.GoToAsync($"{nameof(PgManageCarPark)}/{nameof(PgAddPay)}", true, send);
    //}
    
    //[RelayCommand]
    //async Task GoToManageCarPark()
    //{
    //    await Shell.Current.GoToAsync(nameof(PgManageCarPark), true);
    //}
    
    //[RelayCommand]
    //async Task GoToShare()
    //{
    //    await Shell.Current.GoToAsync("", true);
    //}
    
    //[RelayCommand]
    //async Task GoToManageVehicles()
    //{
    //    await Shell.Current.GoToAsync(nameof(PgManageContracts), true);
    //}
    
    //[RelayCommand]
    //async Task GoToManageUser()
    //{
    //    await Shell.Current.GoToAsync(nameof(PgManageUser), true);
    //}

    #region Extra
    protected override void OnActivated()
    {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register<PgHomeViewModel, string, string>(this, nameof(HasParkContracts),(r, m) =>
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
    #endregion
}
