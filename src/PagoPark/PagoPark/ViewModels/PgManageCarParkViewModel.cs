using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PagoPark.Models;
using PagoPark.Services;
using PagoPark.Views;
using System.Collections.ObjectModel;

namespace PagoPark.ViewModels;

public partial class PgManageCarParkViewModel : ObservableRecipient
{
    //readonly ILiteDbCarParkRecordService carParkRecordServ;
    //readonly ILiteDbVehiclesServices vehiclesServ;

    //public PgManageCarParkViewModel(ILiteDbCarParkRecordService carParkRecordService, ILiteDbVehiclesServices vehiclesServices)
    //{
    //    IsActive = true;
    //    carParkRecordServ = carParkRecordService;
    //    vehiclesServ = vehiclesServices;

    //    Payment = carParkRecordServ.Any()
    //        ? new(carParkRecordServ.GetThisWeek()
    //        .Select(x => new CarParkRecordDemoView(x, vehiclesServ.GetById(x.Id))).Reverse())
    //        :new();
    //}

    //[ObservableProperty]
    //ObservableCollection<CarParkRecordDemoView> payment;

    //[ObservableProperty]
    //CarParkRecordDemoView selectedPay;

    //[RelayCommand]
    //async Task GotoPay()
    //{
    //    var send = new Dictionary<string, object> { { nameof(PgAddPay), vehiclesServ.GetAll().Reverse().ToArray() } };
    //    await Shell.Current.GoToAsync(nameof(PgAddPay), true, send);
    //}

    //[RelayCommand]
    //async Task GotoDetail() => await Shell.Current.GoToAsync(nameof(PgAddPay), true);

    //[RelayCommand]
    //void Delete() => carParkRecordServ.Delete(SelectedPay.Id);

    //#region Extra
    //protected override void OnActivated()
    //{
    //    base.OnActivated();
    //    WeakReferenceMessenger.Default.Register<PgManageCarParkViewModel, CarParkRecordDemo, string>(this, nameof(PgAddPay), (r, m) =>
    //    {
    //        r.Payment.Insert(0, new(m, vehiclesServ.GetById(m.Id)));
    //    });
    //}
    //#endregion
}
