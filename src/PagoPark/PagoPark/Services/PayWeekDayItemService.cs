using PagoPark.Models;

namespace PagoPark.Services;

public class PayWeekDayItemService
{
    private readonly List<ParkContract> _contracts;
    public PayWeekDayItemService(List<ParkContract> contracts)
    {
        _contracts = contracts;
    }

    public IEnumerable<PayWeekDayItem> GeneratePayWeekDayItems()
    {
        var payWeekDayItems = new List<PayWeekDayItem>();
        for (int i = 0; i < 7; i++)
        {
            var contractsDueToday = _contracts.Where(c => c.WeekFrequency.Contains(i)).ToList();
            var payContracts = contractsDueToday.Select(c => new PayContract { PayDate = DateTime.Today, Pay = c.PayPerFrequency, Contract = c });
            payWeekDayItems.Add(new PayWeekDayItem { DayOfWeek = ((DayOfWeek)i).ToString(), Contracts = payContracts });
        }
        return payWeekDayItems;
    }

    public void UpdateContractPay(string contractId, double newPay)
    {
        var contract = _contracts.FirstOrDefault(c => c.Id == contractId);
        if (contract != null)
        {
            contract.PayPerFrequency = newPay;
        }
    }
}