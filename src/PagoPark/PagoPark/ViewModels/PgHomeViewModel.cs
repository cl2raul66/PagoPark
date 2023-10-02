using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PagoPark.Services;
using PagoPark.Views;

namespace PagoPark.ViewModels;

public partial class PgHomeViewModel : ObservableRecipient
{
    readonly ILiteDbVehiclesServices vehiclesServ;

    public PgHomeViewModel(ILiteDbVehiclesServices vehiclesServices)
    {
        IsActive = true;
        vehiclesServ = vehiclesServices;
        HasVehicle = vehiclesServ.Any();
    }

    [ObservableProperty]
    bool hasVehicle;

    [RelayCommand]
    async Task GoToPay()
    {
        await Shell.Current.GoToAsync(nameof(PgPay), true);
    }
    
    [RelayCommand]
    async Task GoToDetailPay()
    {
        await Shell.Current.GoToAsync(nameof(PgDetailPay), true);
    }
    
    [RelayCommand]
    async Task GoToShare()
    {
        await Shell.Current.GoToAsync("", true);
    }
    
    [RelayCommand]
    async Task GoToManageVehicles()
    {
        await Shell.Current.GoToAsync(nameof(PgManageVehicles), true);
    }
    
    [RelayCommand]
    async Task GoToManageUser()
    {
        await Shell.Current.GoToAsync(nameof(PgManageUser), true);
    }

    #region Extra
    protected override void OnActivated()
    {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register<PgHomeViewModel, string, string>(this, nameof(HasVehicle),(r, m) =>
        {
            _ = bool.TryParse(m, out bool resul);
            r.HasVehicle = resul;
        });
    }
    #endregion
}
