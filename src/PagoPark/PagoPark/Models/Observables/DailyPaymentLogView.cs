using CommunityToolkit.Mvvm.ComponentModel;
using PagoPark.Services;

namespace PagoPark.Models.Observables;

[ObservableObject]
public partial class DailyPaymentLogView : DailyPaymentLog
{

    readonly ILiteDbParkContractServices parkContractServ;

    public DailyPaymentLogView(ILiteDbParkContractServices liteDbParkContractServices, DailyPaymentLog dailyPaymentLog) : base(dailyPaymentLog.PaymentDate, dailyPaymentLog.ParkContractId, dailyPaymentLog.RecordDate, dailyPaymentLog.Amount, dailyPaymentLog.Note)
    {
        Id = dailyPaymentLog.Id;
        parkContractServ = liteDbParkContractServices;
    }

    //public DailyPaymentLogView(ILiteDbParkContractServices liteDbParkContractServices)
    //{
    //    parkContractServ = liteDbParkContractServices;
    //}

    public override string ToString()
    {
        ParkContract contract = parkContractServ.GetById(ParkContractId);
        return $"{contract}";
    }
}
