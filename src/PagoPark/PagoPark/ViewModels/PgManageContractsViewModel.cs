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
    readonly ILiteDbParkContractServices parkContractServ;

    public PgManageContractsViewModel(ILiteDbParkContractServices parkContractServices)
    {
        IsActive = true;
        parkContractServ = parkContractServices;

        if (!parkContractServ.Any())
        {

            parkContractServ.Insert(new(new("C211BKG", "Miguel", Tools.TypeVehicleFunction.Load, Tools.TypeLoadCapacity.Medium), new int[] { 6 }, 100));
            parkContractServ.Insert(new(new("C643BLW", "Miguel", Tools.TypeVehicleFunction.Load, Tools.TypeLoadCapacity.Medium), new int[] { 0, 3, 5 }, 100));
            parkContractServ.Insert(new(new("C770BWS", "Ramiro", Tools.TypeVehicleFunction.Load, Tools.TypeLoadCapacity.Heavy), new int[] { 0, 3 }, 50));
            parkContractServ.Insert(new(new("C113308", "Ramiro", Tools.TypeVehicleFunction.Load, Tools.TypeLoadCapacity.Heavy), new int[] { 1, 4 }, 50));
            parkContractServ.Insert(new(new("C166BTS", "Israel Choi", Tools.TypeVehicleFunction.Load, Tools.TypeLoadCapacity.Medium), new int[] { 1, 5 }, 125));
        }

        ParkContracts = parkContractServ.Any() ? new(parkContractServ.GetAll().Reverse()) : new();
    }

    [ObservableProperty]
    ObservableCollection<ParkContract> parkContracts;

    [ObservableProperty]
    ParkContract selectedContract;

    [RelayCommand]
    async Task GotoAddcontract() => await Shell.Current.GoToAsync(nameof(PgAddContract), true);

    [RelayCommand]
    void Delete() => parkContractServ.Delete(SelectedContract.Id);

    #region Extra
    protected override void OnActivated()
    {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register<PgManageContractsViewModel, ParkContract, string>(this, nameof(PgAddContract), (r, m) =>
        {
            if (!string.IsNullOrEmpty(m.Id) && parkContractServ.Exist(m.Id))
            {
                return;
            }

            if (parkContractServ.Insert(m))
            {
                r.ParkContracts.Insert(0, m);
                WeakReferenceMessenger.Default.Send(true.ToString(), nameof(PgManageContracts));
            }
        });
    }
    #endregion
}
