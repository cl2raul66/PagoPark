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
    List<ParkContract> _contracts;

    public PgPayViewModel(ILiteDbParkContractServices parkContractServices)
    {
        IsActive = true;
        parkContractServ = parkContractServices;

        if (parkContractServ.Any())
        {
            _contracts = new(parkContractServ.GetAll());
            GeneratePayWeekDayItems();
            //PayWeekDayItems = new();

            //foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            //{
            //    var d = parkContractServ.GetAll();
            //    PayWeekDayItems.Add(new()
            //    {
            //        DayOfWeek = day.ToString(),
            //        Contracts = d.Where(x => x.WeekFrequency.Contains((int)day)).Select(x => new PayContract() { Contract = x })
            //    });
            //}
            CurrentPayWeekDayItem = PayWeekDayItems[(int)DateTime.Now.DayOfWeek];
        }
    }

    [ObservableProperty]
    ObservableCollection<PayWeekDayItem> payWeekDayItems;

    [ObservableProperty]
    PayWeekDayItem currentPayWeekDayItem;

    [ObservableProperty]
    PayContract selectedContract;

    [RelayCommand]
    async Task GoToSetPay()
    {
        Dictionary<string, object> senderObject = new() { { nameof(SelectedContract),SelectedContract } };
        await Shell.Current.GoToAsync(nameof(PgAddPay), true, senderObject);
    }

    #region Extra
    protected override void OnActivated()
    {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register<PgPayViewModel, PayContract, string>(this, nameof(PgAddPay), (r, m) =>
        { 
            UpdateContractPay(m.Contract.Id, m.Pay);
            GeneratePayWeekDayItems();
        });
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(CurrentPayWeekDayItem))
        {
            SelectedContract = null;
        }
    }

    void GeneratePayWeekDayItems()
    {
        var items = new List<PayWeekDayItem>();
        for (int i = 0; i < 7; i++)
        {
            var contractsDueToday = _contracts.Where(c => c.WeekFrequency.Contains(i)).ToList();
            var pContracts = contractsDueToday.Select(c => new PayContract { PayDate = DateTime.Today, Pay = c.PayPerFrequency, Contract = c });
            items.Add(new PayWeekDayItem { DayOfWeek = ((DayOfWeek)i).ToString(), Contracts = pContracts });
        }

        PayWeekDayItems = new(items);
    }

    void UpdateContractPay(string contractId, double newPay)
    {
        var contract = _contracts.FirstOrDefault(c => c.Id == contractId);
        if (contract != null)
        {
            contract.PayPerFrequency = newPay;
        }
    }
    #endregion
}
