namespace PagoPark.Models;

public class DailyPaymentLog
{
    public string Id { get; set; }
    public DateTime PaymentDate { get; set; }
    public string ParkContractId { get; set; }
    public DateTime RecordDate { get; set; }
    public decimal Amount { get; set; }
    public string Note { get; set; }

    public DailyPaymentLog() { }
    public DailyPaymentLog(DateTime paymentdate, string parkcontractid, DateTime recorddate, decimal amount, string note = "")
    {
        PaymentDate = paymentdate;
        ParkContractId = parkcontractid;
        RecordDate = recorddate;
        Amount = amount;
        Note = string.IsNullOrEmpty(note) ? string.Empty : note;
    }
}
