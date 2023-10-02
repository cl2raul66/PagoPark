using PagoPark.Tools;

namespace PagoPark.Models;

public class VehicleDemo : Vehicle
{
    public int[] WeekFrequency { get; set; }
    public double PayPerFrequency { get; set; }
    public string Owner { get; set; }

    public VehicleDemo() { }
    public VehicleDemo(string licenseplate, TypeVehicleFunction typefunction, TypeLoadCapacity typecapacity, int[] weekfrequency, double payperfrequency, string owner)
    {
        LicensePlate = licenseplate;
        TypeFunction = typefunction;
        TypeCapacity = typecapacity;
        WeekFrequency = weekfrequency;
        PayPerFrequency = payperfrequency;
        Owner = owner;
    }

    public override string ToString()
    {
        return $"{LicensePlate} {Owner}";
    }
}
