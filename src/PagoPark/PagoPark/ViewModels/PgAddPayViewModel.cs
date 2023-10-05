using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PagoPark.Models;
using PagoPark.Views;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PagoPark.ViewModels;

[QueryProperty(nameof(VehicleList),nameof(PgAddPay))]
public partial class PgAddPayViewModel : ObservableValidator
{
    public PgAddPayViewModel()
    {
        
    }

    [ObservableProperty]
    VehicleDemo[] vehicleList;

    [ObservableProperty]
    [Required]
    VehicleDemo selectedVehicle;

    [ObservableProperty]
    [Required]
    string pay;

    [ObservableProperty]
    string observations;

    [ObservableProperty]
    bool isVisibleInfo;

    [RelayCommand]
    async Task Save() {
        ValidateAllProperties();

        if (HasErrors)
        {
            IsVisibleInfo = true;
            await Task.Delay(5000);
            IsVisibleInfo = false;
            return;
        }

        CarParkRecord newRecord = null;

        WeakReferenceMessenger.Default.Send(newRecord, nameof(PgAddPay));
    }
    
    [RelayCommand]
    async Task Cancel() => await Shell.Current.GoToAsync("..", true);

    #region Extra
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        //if (e.PropertyName == nameof(VehicleList))
        //{
        //    var d = VehicleList;
        //}
    }
    #endregion
}
