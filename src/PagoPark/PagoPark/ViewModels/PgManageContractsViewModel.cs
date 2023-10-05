using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PagoPark.Models;
using PagoPark.Services;
using PagoPark.Views;
using System.Collections.ObjectModel;

namespace PagoPark.ViewModels;

public partial class PgManageContractsViewModel : ObservableRecipient
{
    readonly ILiteDbVehiclesServices vehiclesServ;

    public PgManageContractsViewModel(ILiteDbVehiclesServices vehiclesServices)
    {
        IsActive = true;
        vehiclesServ = vehiclesServices;
        Vehicles = vehiclesServ.Any() ? new(vehiclesServ.GetAll().Reverse()) : new();
    }

    [ObservableProperty]
    ObservableCollection<VehicleDemo> vehicles;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnableDelete))]
    VehicleDemo selectedVehicle;

    public bool IsEnableDelete => SelectedVehicle is not null;

    [RelayCommand]
    async Task GotoAddvehicle() => await Shell.Current.GoToAsync(nameof(PgAddVehicle), true);

    [RelayCommand]
    void Delete() => vehiclesServ.Delete(SelectedVehicle.LicensePlate);

    #region Extra
    protected override void OnActivated()
    {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register<PgManageContractsViewModel, VehicleDemo, string>(this, nameof(PgAddVehicle), (r, m) =>
        {
            if (vehiclesServ.Exist(m.LicensePlate))
            {
                return;
            }

            if (vehiclesServ.Insert(m))
            {
                r.Vehicles.Insert(0, m);
            }
        });
    }
    #endregion
}
