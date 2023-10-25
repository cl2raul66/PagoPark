using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PagoPark.Models.Observables;
using PagoPark.Views;
using System.ComponentModel.DataAnnotations;

namespace PagoPark.ViewModels;

[QueryProperty(nameof(SelectedPaymentLog), nameof(SelectedPaymentLog))]
public partial class PgAddPayViewModel : ObservableValidator
{
    [ObservableProperty]
    DailyPaymentLogView selectedPaymentLog;

    [ObservableProperty]
    [Required]
    string pay;

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

        SelectedPaymentLog.RecordDate = DateTime.Now;
        SelectedPaymentLog.Amount = double.Parse(Pay);
        SelectedPaymentLog.Note = Observations;

        WeakReferenceMessenger.Default.Send(SelectedPaymentLog, nameof(PgAddPay));
        await Cancel();
    }

    [RelayCommand]
    async Task Cancel() => await Shell.Current.GoToAsync("..", true);
}
