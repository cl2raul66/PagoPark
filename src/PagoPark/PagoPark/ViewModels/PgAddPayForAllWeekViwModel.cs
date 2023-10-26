using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PagoPark.Models;
using PagoPark.Views;
using System.ComponentModel.DataAnnotations;

namespace PagoPark.ViewModels;

[QueryProperty(nameof(ParkContracts), nameof(ParkContracts))]
public partial class PgAddPayForAllWeekViwModel : ObservableValidator
{
    [ObservableProperty]
    List<ParkContract> parkContracts;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AmountTotal))]
    [Required]
    ParkContract selectedParkContract;

    public string AmountTotal => SelectedParkContract is null
        ? "No contract selected!"
        : $"{SelectedParkContract.VehicleClient} - Total:  {SelectedParkContract.WeekFrequency.Length * SelectedParkContract.PayPerFrequency}";

    [ObservableProperty]
    [Required]
    string amonunt;

    [ObservableProperty]
    string observations;

    [ObservableProperty]
    bool isVisibleInfo;

    [RelayCommand]
    async Task Save()
    {
        ValidateAllProperties();

        if (HasErrors)
        {
            IsVisibleInfo = true;
            await Task.Delay(5000);
            IsVisibleInfo = false;
            return;
        }

        WeakReferenceMessenger.Default.Send(Guid.NewGuid().ToString(), nameof(PgAddPayForAllWeek));
        await Cancel();
    }

    [RelayCommand]
    async Task Cancel() => await Shell.Current.GoToAsync("..", true);
}
