namespace PagoPark.Models;

public class PayWeekDayItem
{
    public string DayOfWeek { get; set; }
    public IEnumerable<PayContract> Contracts { get; set; }
}

public class PayContract
{
    public DateTime? PayDate { get; set; }
    public double Pay { get; set; }
    public ParkContract Contract { get; set; }
}
