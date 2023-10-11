using LiteDB;
using PagoPark.Tools;

namespace PagoPark.Models;

public class Vehicle
{
    [BsonId]
    public string LicensePlate { get; set; }
    public TypeVehicleFunction TypeFunction { get; set; }
    public TypeLoadCapacity TypeCapacity { get; set; }
    public string Owner { get; set; }

    public Vehicle(){ }
    public Vehicle(string licenseplate, string owner, TypeVehicleFunction typefunction, TypeLoadCapacity typecapacity)
    {
        LicensePlate = licenseplate;
        Owner = owner;
        TypeFunction = typefunction;
        TypeCapacity = typecapacity;
    }

    public override string ToString()
    {
        return $"{LicensePlate} | {Owner}";
    }
}