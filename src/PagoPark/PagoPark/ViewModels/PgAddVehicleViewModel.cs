using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PagoPark.Models;
using PagoPark.Tools;
using PagoPark.Views;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace PagoPark.ViewModels;

public partial class PgAddVehicleViewModel : ObservableValidator
{

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnableSave))]
    [Required]
    [MinLength(6)]
    string licenseplate;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnableSave))]
    [Required]
    string payperfrequency;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnableSave))]
    [Required]
    [MinLength(2)]
    string owner;

    [ObservableProperty]
    IEnumerable<string> typefunctions = Enum.GetNames(typeof(TypeVehicleFunction));

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnableSave))]
    [Required]
    string selectedTypefunction;

    [ObservableProperty]
    IEnumerable<string> typecapacities = Enum.GetNames(typeof(TypeLoadCapacity));

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnableSave))]
    [Required]
    string selectedTypecapacity;

    [ObservableProperty]
    IEnumerable<string> daysOfWeek = Enum.GetNames(typeof(DayOfWeek));

    [ObservableProperty]
    [MinLength(1)]
    ObservableCollection<object> selectedDaysOfWeek = new();

    public bool IsEnableSave => !string.IsNullOrEmpty(Licenseplate)
        && !string.IsNullOrEmpty(Owner)
        && !string.IsNullOrEmpty(Payperfrequency) && !string.IsNullOrEmpty(SelectedTypefunction)
        && !string.IsNullOrEmpty(SelectedTypecapacity);

    [ObservableProperty]
    bool isVisibleInfo;

    [RelayCommand]
    async Task Save()
    {
        bool resul = await SendVehicle();
        if (resul)
        {
            Licenseplate = null;
            Owner = null;
            Payperfrequency = null;
            SelectedTypefunction = null;
            SelectedTypecapacity = null;
            SelectedDaysOfWeek = new();
            ClearErrors();
        }
    }

    [RelayCommand]
    private async Task SaveClose()
    {
        bool resul = await SendVehicle();
        if (resul)
        {
            await Cancel();
        }
    }

    [RelayCommand]
    async Task Cancel() => await Shell.Current.GoToAsync("..", true);

    #region Extra
    async Task<bool> SendVehicle()
    {
        ValidateAllProperties();

        if (HasErrors)
        {
            IsVisibleInfo = true;
            await Task.Delay(5000);
            IsVisibleInfo = false;
            return false;
        }

        var Weekfrequency = SelectedDaysOfWeek.Select(x => (int)Enum.Parse<DayOfWeek>(x as string)).ToArray();

        VehicleDemo newVehicle = new(Licenseplate.Trim(), Enum.Parse<TypeVehicleFunction>(SelectedTypefunction), Enum.Parse<TypeLoadCapacity>(SelectedTypecapacity), Weekfrequency, double.Parse(Payperfrequency.Trim()), Owner.Trim());
        return WeakReferenceMessenger.Default.Send(newVehicle, nameof(PgAddVehicle)) is not null;
    }
    #endregion
}
