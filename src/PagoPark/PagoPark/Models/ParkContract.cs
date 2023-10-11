using LiteDB;

namespace PagoPark.Models;

public class ParkContract
{
    [BsonId]
    public string Id { get; set; }
    public Vehicle VehicleClient { get; set; }
    public int[] WeekFrequency { get; set; }
    public double PayPerFrequency { get; set; }
    public bool IsCurrent { get; set; }

    public ParkContract() { }
    public ParkContract(Vehicle vehicleclient, int[] weekfrequency, double payperfrequency, bool iscurrent = true)
    {
        VehicleClient = vehicleclient;
        WeekFrequency = weekfrequency;
        PayPerFrequency = payperfrequency;
        IsCurrent = iscurrent;
    }

    public override string ToString()
    {
        return $"{VehicleClient} - {PayPerFrequency:0.00}";
    }
}
